using Core.Exceptions.TagCategories;

namespace Core.Features.TagCategories.Queries;

public static class GetTagCategoryAssociatedEntities
{
    public class Validator : AbstractValidator<Query>
    {
        public Validator(
            ITenantRepository tenantRepository,
            ITagCategoryRepository tagCategoryRepository)
        {
            RuleFor(q => q.TenantCode)
                .NotEmptyAndRequired()
                .TenantExists(tenantRepository);
            
            RuleFor(q => q.TagCategoryCode)
                .NotEmptyAndRequired()
                .MustAsync(async (query, tagCategoryCode, cancellationToken) =>
                {
                    var tagCategory = await tagCategoryRepository.GetByAsync(query.TenantCode, tagCategoryCode, cancellationToken);
                    
                    return tagCategory is not null;
                })
                .WithMessage((_, tagCategoryCode) => new TagCategoryNotFoundException(tagCategoryCode).Message);
        }
    }

    public class Query(int tenantCode, string tagCategoryCode) : IRequest<IEnumerable<TagCategoryAssociatedEntitiesDto>>
    {
        public int TenantCode { get; set; } = tenantCode;

        public string TagCategoryCode { get; set; } = tagCategoryCode;
    }
 
    public class Handler(
        ITagRepository tagRepository,
        IProductRepository productRepository)
        : IRequestHandler<Query, IEnumerable<TagCategoryAssociatedEntitiesDto>>
    {
        public async Task<IEnumerable<TagCategoryAssociatedEntitiesDto>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            var tags = await tagRepository.GetAllAsync(
                new TagFilter(request.TenantCode)
                {
                    TagCategoryCode = request.TagCategoryCode
                },
                cancellationToken);

            var tagCodes = tags.Select(t => t.Code).ToArray();
            
            var products = await productRepository.GetAllAsync(new(request.TenantCode)
            {
                TagCodes = tagCodes
            }, cancellationToken);

            if (!products.Any())
            {
                throw new TagCategoryHasNoAssociatedEntitiesException(request.TagCategoryCode, tagCodes);
            }

            return tags.Select(t => new TagCategoryAssociatedEntitiesDto
            {
                Tag = t.Code,
                Products = products.Where(p => p.TagCodes.Contains(t.Code))
            });
        }
    }

    public class TagCategoryAssociatedEntitiesDto
    {
        public string Tag { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}