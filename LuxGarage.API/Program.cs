using LuxGarage.API.Data;
using LuxGarage.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddStoreDb();
builder.Services.AddRepositories();

var app = builder.Build();
app.MigrateDb();

// app.UseHttpsRedirection();

app.Run();
