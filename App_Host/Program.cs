using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var basketApi = builder.AddProject<Basket_API>("basket");

var orderApi = builder.AddProject<Order_API>("order");

var productApi = builder.AddProject<Product_API>("product").WithHttpsEndpoint(8001);

var identityApi = builder.AddProject<Identity_API>("identity");

builder.Build().Run();
