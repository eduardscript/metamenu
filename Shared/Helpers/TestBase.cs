using AutoFixture;

namespace Shared.Helpers;

public abstract class TestBase
{
    protected static readonly Fixture Fixture = new();
}