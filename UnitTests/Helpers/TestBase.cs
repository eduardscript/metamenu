using FluentValidation.Results;

namespace UnitTests.Helpers;

public class TestBase
{
    protected static readonly Fixture Fixture = new();

    protected readonly ITenantRepository TenantRepositoryMock = Substitute.For<ITenantRepository>();

    protected readonly ITagCategoryRepository TagCategoryRepositoryMock = Substitute.For<ITagCategoryRepository>();

    protected readonly ITagRepository TagRepositoryMock = Substitute.For<ITagRepository>();

    protected readonly IProductRepository ProductRepositoryMock = Substitute.For<IProductRepository>();

    protected readonly IUserRepository UserRepositoryMock = Substitute.For<IUserRepository>();
}
