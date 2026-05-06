using LuxGarage.API.Data;
using LuxGarage.API.Extensions;
using LuxGarage.API.Profiles;

var builder = WebApplication.CreateBuilder(args);

builder.AddStoreDb();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.MigrateDb();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.MapControllers();

app.Run();
