using LuxGarage.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddStoreDb();
builder.Services.AddCorsPolicy();
builder.Services.AddApplicationServices();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerWithJwtAuth();

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
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
