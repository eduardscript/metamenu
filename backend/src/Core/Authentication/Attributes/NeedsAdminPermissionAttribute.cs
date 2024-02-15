namespace Core.Authentication.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class NeedsAdminPermissionAttribute : Attribute;