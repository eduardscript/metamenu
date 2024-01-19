﻿using Core.Features.Tags.Commands;

namespace IntegrationTests.Features.Tags.Commands;

[TestClass]
public class RenameTagCodeTests : BaseIntegrationTest
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

        var handler = new RenameTagCode.Handler(_tagRepository);

        // Act
        await handler.Handle(
            new RenameTagCode.Command(tenant.Code, oldTag.Code, newTagCode),
            default);

        // Assert
        var oldTagCategoryExists =
            await _tagRepository.ExistsAsync(tenant.Code, oldTag.Code, default);
        oldTagCategoryExists.Should().BeFalse();

        // TODO:EC Fix this test to use just the tagCode instead of the tagCodes array
        var newTagCategoryExists =
            await _tagRepository.ExistsAsync(tenant.Code, new[] { newTagCode }, default);
        newTagCategoryExists.Should().BeTrue();

        newTagCode.Should().NotBeEquivalentTo(oldTag.Code);
    }
}