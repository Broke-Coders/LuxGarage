var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Włączenie obsługi plików statycznych w folderze wwwroot
app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();
