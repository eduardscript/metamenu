using HotChocolate.Types.Descriptors;
using Infra;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Presentation;
using Presentation.Configuration;
using Presentation.Conventions;
using Presentation.Filters;
using Presentation.Mutations;
using Presentation.Queries;

var builder = WebApplication.CreateSlimBuilder(args);


var signingKey = new SymmetricSecurityKey(
    "2e6f9b0d5885b6010f9167787445617f553a735f"u8.ToArray());

builder.Services
    .ConfigureOptions<JwtOptions>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer()
    .Services
    .AddAuthorization();

builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType(d => d.Name(RootTypes.Query))
    .AddTypeExtension<TenantQueries>()
    .AddTypeExtension<TagCategoryQueries>()
    .AddTypeExtension<TagQueries>()
    .AddTypeExtension<ProductQueries>()
    .AddTypeExtension<UserQueries>()
    .AddMutationType(d => d.Name(RootTypes.Mutation))
    .AddTypeExtension<TenantMutations>()
    .AddTypeExtension<TagCategoryMutations>()
    .AddTypeExtension<TagMutations>()
    .AddTypeExtension<ProductMutations>()
    .AddTypeExtension<UserMutations>()
    .AddConvention<INamingConventions, CustomNamingConvention>()
    .AddErrorFilter<ErrorFilter>();

builder.Services
    .AddApplication()
    .AddInfra(builder.Configuration);

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();

app.Run();