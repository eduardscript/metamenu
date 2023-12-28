namespace Core.Validators;

public static class CustomValidators
{
    public static IRuleBuilderOptions<T, IEnumerable<TElement>> Unique<T, TElement>(this IRuleBuilder<T, IEnumerable<TElement>> ruleBuilder)
    {
        return ruleBuilder.Must((rootObject, list, context) =>
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
            .WithMessage("Duplicate items found: {DuplicateItems}.");
    }
}
