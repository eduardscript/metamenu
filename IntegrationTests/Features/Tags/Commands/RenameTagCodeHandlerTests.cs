using Core.Features.TagCategories.Commands;
using Core.Features.Tags.Commands;

namespace IntegrationTests.Features.Tags.Commands;

[TestClass]
public class RenameTagCodeHandlerTests : IntegrationTestBase
{
    private readonly ITagRepository _tagRepository = GetService<ITagRepository>();
    private readonly ITenantRepository _tenantRepository = GetService<ITenantRepository>();

    [TestMethod]
    public async Task Handle_RenamesTagCategoryInDatabase()
    {
        // Arrange
        var tenant = await MongoDbFixture.CreateTenantAsync();
        var oldTag = Fixture
            .Build<Tag>()
            .With(tc => tc.TenantCode, tenant.TenantCode)
            .Create();

        var newTagCode = Fixture.Create<string>();

        await _tagRepository.CreateAsync(oldTag, default);

        var handler = new RenameTagCodeHandler.Handler(_tenantRepository, _tagRepository);

        // Act
        await handler.Handle(
            new RenameTagCodeHandler.Command(tenant.TenantCode, oldTag.TagCode, newTagCode),
            default);

        // Assert
        var oldTagCategoryExists =
            await _tagRepository.ExistsAsync(tenant.TenantCode, oldTag.TagCode, default);
        oldTagCategoryExists.Should().BeFalse();

        var newTagCategoryExists =
            await _tagRepository.ExistsAsync(tenant.TenantCode, newTagCode, default);
        newTagCategoryExists.Should().BeTrue();

        newTagCode.Should().NotBeEquivalentTo(oldTag.TagCode);
    }
}