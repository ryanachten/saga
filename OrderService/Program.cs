using Common.Extensions;
using OrderService.Extensions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddRabbitMq(builder.Configuration, typeof(Program).Assembly);

builder.Services.AddOrderStrategies();

builder.Services.AddHttpClients(builder.Configuration);

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
