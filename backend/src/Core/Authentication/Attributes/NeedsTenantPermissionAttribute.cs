namespace Core.Authentication.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class NeedsTenantPermissionAttribute : Attribute;