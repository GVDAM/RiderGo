using RiderGo.API.Common.Api;
using RiderGo.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();
builder.AddDocumentation();
builder.AddDataContexts();
builder.AddCrossOrigin();
builder.AddServices();
builder.AddPubSubConfiguration();

builder.Services.AddMediatRConfiguration();

var app = builder.Build();

app.ApplyMigrations();

if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
    app.ConfigureDevEnvironment();

app.MapEndpoints();

app.Run();
