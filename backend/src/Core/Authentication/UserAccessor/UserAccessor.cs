﻿using System.Security.Claims;

namespace Core.Authentication.UserAccessor;

public class UserAccessor : IUserAccessor
{
    public ClaimsPrincipal ClaimsPrincipal { get; set; } = null!;
}