using LuxGarage.API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddStoreDb();
var app = builder.Build();
//app.MigrateDb();

app.UseHttpsRedirection();

app.Run();
