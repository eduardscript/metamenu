using HotChocolate.Types.Descriptors;
using Infra;
using Infra.Configuration;
using Presentation;
using Presentation.Conventions;
using Presentation.Mutations;
using Presentation.Queries;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services
    .AddApplication()
    .AddInfra(builder.Configuration.GetRequiredSection("MongoDb").Get<MongoConfiguration>()!)
    .AddGraphQLServer()
    .AddQueryType(d => d.Name(RootTypes.Query))
    .AddTypeExtension<TenantQueries>()
    .AddTypeExtension<TagCategoryQueries>()
    .AddTypeExtension<TagQueries>()
    .AddTypeExtension<ProductQueries>()
    .AddMutationType(d => d.Name(RootTypes.Mutation))
    .AddTypeExtension<TenantMutations>()
    .AddTypeExtension<TagCategoryMutations>()
    .AddTypeExtension<TagMutations>()
    .AddTypeExtension<ProductMutations>()
    .AddConvention<INamingConventions, CustomNamingConvention>();

var app = builder.Build();

app.MapGraphQL();

app.Run();