var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "u + me = <3");

app.Run();
