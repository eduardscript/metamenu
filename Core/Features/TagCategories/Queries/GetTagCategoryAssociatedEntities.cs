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
                .NotEmptyAndRequired();
                
            RuleFor(q => q.TenantCode)
                .TenantExists(tenantRepository);
            
            RuleFor(q => q.TagCategoryCode)
                .NotEmptyAndRequired();

            RuleFor(q => q.TagCategoryCode)
                .MustAsync(async (query, tagCategoryCode, cancellationToken) =>
                {
                    var tagCategory = await tagCategoryRepository.GetByAsync(query.TenantCode, tagCategoryCode, cancellationToken);
                    
                    return tagCategory is not null;
                })
                .WithMessage((_, tagCategoryCode) => new TagCategoryNotFoundException(tagCategoryCode).Message);
        }
    }

    public class Query : IRequest<IEnumerable<TagCategoryAssociatedEntitiesDto>>
    {
        public Query(int tenantCode, string tagCategoryCode)
        {
            TenantCode = tenantCode;
            TagCategoryCode = tagCategoryCode;
        }

        public int TenantCode { get; set; }

        public string TagCategoryCode { get; set; }
    }

    public class Handler(
        ITenantRepository tenantRepository,
        ITagCategoryRepository tagCategoryRepository,
        ITagRepository tagRepository,
        IProductRepository productRepository)
        : IRequestHandler<Query, IEnumerable<TagCategoryAssociatedEntitiesDto>>
    {
        public async Task<IEnumerable<TagCategoryAssociatedEntitiesDto>> Handle(Query request,
            CancellationToken cancellationToken)
        {
            var tags = await tagRepository.GetAll(
                new ITagRepository.TagFilter(request.TenantCode, request.TagCategoryCode),
                cancellationToken);

            var tagCodes = tags.Select(t => t.TagCode).ToArray();
            var products = await productRepository.GetAllAsync(new(request.TenantCode, tagCodes), cancellationToken);

            if (!products.Any())
            {
                throw new TagCategoryHasNoAssociatedEntitiesException(request.TagCategoryCode, tagCodes);
            }

            return tags.Select(t => new TagCategoryAssociatedEntitiesDto
            {
                Tag = t.TagCode,
                Products = products.Where(p => p.TagCodes.Contains(t.TagCode))
            });
        }
    }

    public class TagCategoryAssociatedEntitiesDto
    {
        public string Tag { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}