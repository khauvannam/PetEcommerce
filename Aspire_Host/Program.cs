using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var basketApi = builder.AddProject<Basket_API>("basket");

var orderApi = builder.AddProject<Order_API>("order");

var productApi = builder.AddProject<Product_API>("product");

var identityApi = builder.AddProject<Identity_API>("identity");

var client = builder
    .AddProject<Client_App>("client")
    .WithReference(basketApi)
    .WithReference(orderApi)
    .WithReference(identityApi)
    .WithReference(productApi);

builder.Build().Run();
