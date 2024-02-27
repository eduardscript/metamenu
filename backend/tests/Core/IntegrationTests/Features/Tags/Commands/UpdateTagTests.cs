using Core.Features.Tags.Commands;

namespace IntegrationTests.Features.Tags.Commands;

[TestClass]
public class UpdateTagTests : BaseIntegrationTest
{
    private readonly ITagRepository _tagRepository = GetService<ITagRepository>();
    private readonly ITenantRepository _tenantRepository = GetService<ITenantRepository>();

    [TestMethod]
    public async Task Handle_RenamesTagCodeInDatabase()
    {
        // Arrange
        var tenant = await MongoDbFixture.CreateTenantAsync();
        var oldTag = Fixture
            .Build<Tag>()
            .With(tc => tc.TenantCode, tenant.Code)
            .Create();

        var newTagCode = Fixture.Create<string>();

        await _tagRepository.CreateAsync(oldTag, default);

        var handler = new UpdateTag.Handler(_tagRepository);

        // Act
        await handler.Handle(
            new UpdateTag.Command(tenant.Code, oldTag.Code)
            {
                NewTagCode = newTagCode,
            },
            default);

        // Assert
        var oldTagCategoryExists =
            await _tagRepository.ExistsAsync(new(tenant.Code)
            {
                Code = oldTag.Code
            }, default);

        oldTagCategoryExists.Should().BeFalse();

        var newTagCategoryExists =
            await _tagRepository.ExistsAsync(new(tenant.Code)
            {
                Code = newTagCode
            }, default);
        newTagCategoryExists.Should().BeTrue();

        newTagCode.Should().NotBeEquivalentTo(oldTag.Code);
    }
}