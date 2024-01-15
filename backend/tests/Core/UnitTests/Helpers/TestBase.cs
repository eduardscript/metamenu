namespace UnitTests.Helpers;

public class TestBase
{
    protected static readonly Fixture Fixture = new();

    protected readonly ITenantRepository TenantRepositoryMock = Substitute.For<ITenantRepository>();

    protected readonly ITagCategoryRepository TagCategoryRepositoryMock = Substitute.For<ITagCategoryRepository>();

    protected readonly ITagRepository TagRepositoryMock = Substitute.For<ITagRepository>();

    protected readonly IProductRepository ProductRepositoryMock = Substitute.For<IProductRepository>();

    protected readonly IUserRepository UserRepositoryMock = Substitute.For<IUserRepository>();

    protected T Build<T>() where T : class
    {
        var type = typeof(T);
        var validatorConstructor = type.GetConstructors().Single();

        var parameters = validatorConstructor.GetParameters();

        var arguments = new object[parameters.Length];

        var parameterTypes = new Dictionary<Type, object>
        {
            { typeof(ITenantRepository), TenantRepositoryMock },
            { typeof(ITagCategoryRepository), TagCategoryRepositoryMock },
            { typeof(ITagRepository), TagRepositoryMock },
            { typeof(IProductRepository), ProductRepositoryMock },
            { typeof(IUserRepository), UserRepositoryMock }
        };

        foreach (var parameter in parameters)
        {
            parameterTypes.TryGetValue(parameter.ParameterType, out var value);

            if (value is null)
            {
                throw new Exception($"Unknown parameter type: {parameter.ParameterType}");
            }

            arguments.SetValue(value, parameter.Position);
        }

        return Activator.CreateInstance(type, arguments) as T ?? throw new Exception($"Could not create instance of type {type}");
    }
}