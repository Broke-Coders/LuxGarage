using LuxGarage.API.Data;
using LuxGarage.API.Extensions;
using LuxGarage.API.Profiles;
using AutoMapper.Internal;

var builder = WebApplication.CreateBuilder(args);

builder.AddStoreDb();
builder.Services.AddRepositories();
builder.Services.AddCorsPolicy();
builder.Services.AddServices();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<MapperProfile>();
    config.Internal().ForAllMaps((typeMap, mappingExpression) =>
    {
        mappingExpression.MaxDepth(2);
    });
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => {

    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();
app.MigrateDb();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseStaticFiles();

app.MapControllers();

app.Run();
