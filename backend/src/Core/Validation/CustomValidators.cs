using Core.Exceptions.Tenants;
using Humanizer;

namespace Core.Validation;

public static class CustomValidatorsMessages
{
    public static string EntityNotFoundMessage(string entityType, string propertyName, object propertyValue)
    {
        return $"'{entityType}' with '{propertyName}' = '{propertyValue}' was not found.";
    }

    public static string NotEmptyAndRequiredMessage(string propertyName)
    {
        return $"'{propertyName}' is required and must not be empty.";
    }

    public static string GreaterThanZeroMessage(string propertyName)
    {
        var singularPropertyName = propertyName;

        return $"'{singularPropertyName}' must be greater than '0'.";
    }
    
    public static string UniqueMessage(string propertyName, string duplicateItems)
    {
        return $"'{propertyName}' must be unique. Duplicated items found: '{duplicateItems}'.";
    }
}

public static class CustomValidators
{
    public static IRuleBuilderOptions<T, TValue> NotEmptyAndRequired<T, TValue>(
        this IRuleBuilder<T, TValue> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage(CustomValidatorsMessages.NotEmptyAndRequiredMessage("{PropertyName}"));
    }
    
    public static IRuleBuilderOptions<T, int> TenantExists<T>(
        this IRuleBuilder<T, int> ruleBuilder, ITenantRepository tenantRepository)
    {
        return ruleBuilder
            .MustAsync(async (tenantCode, cancellationToken) => await tenantRepository.ExistsAsync(tenantCode, cancellationToken))
            .WithMessage((_, tenantCode) => new TenantNotFoundException(tenantCode).Message);
    }

    public static IRuleBuilderOptions<T, IEnumerable<int>> NotEmptyUniqueAndGreaterThanZero<T>(
        this IRuleBuilder<T, IEnumerable<int>> ruleBuilder)
    {
        return ruleBuilder
            .NotEmptyAndRequired()
            .ForEach(p => p
                .Must((_, value, context) =>
                {
                    var propertyName = context.DisplayName.Singularize();

                    if (value > 0)
                    {
                        return true;
                    }

                    context.MessageFormatter.AppendArgument("SingularPropertyName",
                        propertyName.Singularize().Humanize(LetterCasing.Title));

                    return false;
                })
                .WithMessage(CustomValidatorsMessages.GreaterThanZeroMessage("{SingularPropertyName}")))
            .Unique();
    }

    public static IRuleBuilderOptions<T, IEnumerable<TElement>> Unique<T, TElement>(
        this IRuleBuilder<T, IEnumerable<TElement>> ruleBuilder)
    {
        return ruleBuilder.Must((_, list, context) =>
            {
                if (list == null) return true;

                var duplicates = list.GroupBy(x => x)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

                if (duplicates.Any())
                {
                    context.MessageFormatter.AppendArgument("DuplicateItems", string.Join(", ", duplicates));
                    return false;
                }

                return true;
            })
            .WithMessage(CustomValidatorsMessages.UniqueMessage("{PropertyName}", "{DuplicateItems}"));
    }
}