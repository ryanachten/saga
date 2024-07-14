using Common.Extensions;
using MassTransit;
using OrderService.Extensions;
using OrderService.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddMassTransit(x =>
{
    x.UsingCommonRabbitMq(builder.Configuration, typeof(Program).Assembly);
    x.AddSagaStateMachine<OrderSateMachine, OrderSagaState>()
        .InMemoryRepository(); // TODO: use PGSQL
});

builder.Services.AddOrderStrategies();

builder.Services.AddHttpClients(builder.Configuration);

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
