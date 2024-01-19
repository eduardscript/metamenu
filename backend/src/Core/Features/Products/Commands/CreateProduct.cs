using Core.Features.Products.Shared;

namespace Core.Features.Products.Commands;

public static class CreateProduct
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator(ITenantRepository tenantRepository, ITagRepository tagRepository, IProductRepository productRepository)
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(c => c.TenantCode)
                .ExistsTenant(tenantRepository);

            RuleFor(c => c.TagCodes)
                .NotEmptyAndRequired()
                .Unique()
                .MustAsync(async (command, codes, context, token) =>
                {
                    var tenantCodeProperty = typeof(Command).GetProperty("TenantCode");
                    if (tenantCodeProperty is null)
                    {
                        throw new InvalidOperationException("The type T must have a property named 'TenantCode'.");
                    }

                    var tenantCode = (int)tenantCodeProperty.GetValue(command)!;

                    var existingTags =
                        await tagRepository.GetAllAsync(new TagFilter(tenantCode), token);

                    var invalidTagCodes = command.TagCodes.Except(existingTags.Select(t => t.Code)).ToList();

                    context.MessageFormatter.AppendArgument("InvalidTagCodes", string.Join(", ", invalidTagCodes));

                    return codes.All(c => invalidTagCodes.Contains(c));
                })
                .WithMessage("The following tag codes are invalid: {InvalidTagCodes}");

            RuleFor(c => c.Name)
                .NotEmptyAndRequired();

            RuleFor(c => c.Price)
                .GreaterThanZeroAndRequired();
        }
    }

    public class Command(
        int tenantCode,
        IEnumerable<string> tagCodes,
        string name,
        string? description,
        decimal price) : IRequest<ProductDto>
    {
        public int TenantCode { get; set; } = tenantCode;

        public IEnumerable<string> TagCodes { get; set; } = tagCodes;

        public string Name { get; set; } = name;

        public string? Description { get; set; } = description;

        public decimal Price { get; set; } = price;
    }

    public class Handler(IProductRepository productRepository) : IRequestHandler<Command, ProductDto>
    {
        public async Task<ProductDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var product = new Product(
                request.TenantCode,
                request.Name,
                request.Description,
                request.Price,
                request.TagCodes);

            var newProduct = await productRepository.CreateAsync(product, cancellationToken);

            return newProduct.ToDto();
        }
    }
}