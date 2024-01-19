namespace IntegrationTests.Helpers;

[TestClass]
public abstract class TestBase
{
    [AssemblyInitialize]
    public static void AssemblyInitialize(TestContext context)
    {
        BaseIntegrationTest.AssemblyInitialize(context);     
    }
    
    [AssemblyCleanup]
    public static Task AssemblyCleanup()
    {
        return BaseIntegrationTest.AssemblyCleanup();
    }
}