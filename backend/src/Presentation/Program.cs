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
    .AddCors(options =>
    {
        options.AddPolicy(Clients.Admin,
            policy => policy
                .WithOrigins("http://localhost:3000")
                .WithMethods(HttpMethods.Post)
                .AllowAnyHeader());
    });

builder.Services
    .AddApplication()
    .AddInfra(builder.Configuration);

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors(Clients.Admin);

app.MapGraphQL();

app.Run();