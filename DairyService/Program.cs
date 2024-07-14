using Common.Extensions;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddMassTransit(x =>
{
    x.UsingCommonRabbitMq(builder.Configuration, typeof(Program).Assembly);
});

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
