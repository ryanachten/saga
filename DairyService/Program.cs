using Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddRabbitMq(builder.Configuration, typeof(Program).Assembly);

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
