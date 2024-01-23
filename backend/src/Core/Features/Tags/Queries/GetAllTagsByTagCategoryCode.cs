using Core.Features.Tags.Shared;

namespace Core.Features.Tags.Queries;

public static class GetAllTagsByTagCategoryCode
{
    public class Validator : AbstractValidator<Query>
    {
        public Validator(ITenantRepository tenantRepository, ITagCategoryRepository tagCategoryRepository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(c => c.TenantCode)
                .ExistsTenant(tenantRepository);

            RuleFor(c => c.TagCategoryCode)
                .ExistsTagCategory(tagCategoryRepository);
        }
    }

    public class Query(int tenantCode, string tagCategoryCode) : IRequest<IEnumerable<TagDto>>
    {
        public int TenantCode { get; set; } = tenantCode;

        public string TagCategoryCode { get; set; } = tagCategoryCode;
    }

    public class Handler(ITagRepository tagRepository) : IRequestHandler<Query, IEnumerable<TagDto>>
    {
        public async Task<IEnumerable<TagDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var tags = await tagRepository.GetAllAsync(
                new(request.TenantCode)
                {
                    TagCategoryCode = request.TagCategoryCode
                },
                cancellationToken);

            return tags.ToDto();
        }
    }
}