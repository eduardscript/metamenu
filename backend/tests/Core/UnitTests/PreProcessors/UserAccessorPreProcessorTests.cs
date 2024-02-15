using Core.Authentication.Handlers;
using Core.Authentication.Helpers.Cache;
using Core.Authentication.UserAccessor;
using Core.Exceptions.Extensions;
using Core.PreProcessors;

namespace UnitTests.PreProcessors;

[TestClass]
public class UserAccessorPreProcessorTests
{
    private readonly IUserAccessor _userAccessor = Substitute.For<IUserAccessor>();
    private readonly IPropertyCache _propertyCache = Substitute.For<IPropertyCache>();
    private readonly IServiceProvider _serviceProvider = Substitute.For<IServiceProvider>();
    private readonly TestRequest _request = new();
    
    [TestMethod]
    public async Task Process_Throws_NoHandlerFoundException_WhenHandlerIsNull()
    {
        // Arrange
        var propertyInfo = typeof(TestRequest).GetProperty("TestProperty");
        var attribute = new TestAttribute();

        _propertyCache.GetCachedProperties(typeof(TestRequest)).Returns(new List<CachedPropertyInfo>
        {
            new()
            {
                PropertyInfo = propertyInfo!,
                Attributes = new List<Attribute> { attribute }
            }
        });

        _serviceProvider.GetService(typeof(IAttributeHandler<TestAttribute>))
            .Returns((object)null!);

        var preProcessor = new UserAccessorPreProcessor<TestRequest>(_userAccessor, _propertyCache, _serviceProvider);

        // Act & Assert
        var act = async () => await preProcessor.Process(_request, CancellationToken.None);
        await act.Should().ThrowAsync<NoHandlerFoundException>()
            .WithMessage("No handler found for attribute: TestAttribute");
    }

    [TestMethod]
    public async Task Process_CallsHandleAsync_WhenHandlerExists()
    {
        // Arrange
        var propertyInfo = typeof(TestRequest).GetProperty("TestProperty");
        var attribute = new TestAttribute();

        _propertyCache.GetCachedProperties(typeof(TestRequest)).Returns(new List<CachedPropertyInfo>
        {
            new()
            {
                PropertyInfo = propertyInfo!,
                Attributes = new List<Attribute> { attribute }
            }
        });

        var handler = Substitute.For<IAttributeHandler<TestAttribute>>();
        handler.HandleAsync(_request, _userAccessor, "testValue", CancellationToken.None).Returns(Task.CompletedTask);

        _serviceProvider.GetService(typeof(IAttributeHandler<TestAttribute>)).Returns(handler);

        var preProcessor = new UserAccessorPreProcessor<TestRequest>(_userAccessor, _propertyCache, _serviceProvider);

        // Act
        await preProcessor.Process(_request, CancellationToken.None);

        // Assert
        await handler.Received(1).HandleAsync(_request, _userAccessor, "testValue", CancellationToken.None);
    }


    [TestMethod]
    public async Task Process_DoesNotCallHandleAsync_WhenNoAttributesExist()
    {
        // Arrange
        _propertyCache.GetCachedProperties(typeof(TestRequest)).Returns(new List<CachedPropertyInfo>());

        var preProcessor = new UserAccessorPreProcessor<TestRequest>(_userAccessor, _propertyCache, _serviceProvider);

        // Act
        await preProcessor.Process(_request, CancellationToken.None);

        // Assert
        _serviceProvider.DidNotReceive().GetService(Arg.Any<Type>());
    }

    public class TestRequest
    {
        public string TestProperty { get; set; } = "testValue";
    }

    public class TestAttribute : Attribute;
}