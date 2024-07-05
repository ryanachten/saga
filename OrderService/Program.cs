using OrderService.Clients.DairyClient;
using OrderService.Clients.DeliveryClient;
using OrderService.Clients.NotificationClient;
using OrderService.Clients.ProduceClient;
using OrderService.Extensions;
using MassTransit;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x => x.UsingInMemory((ctx, config) =>
{
    config.ConfigureEndpoints(ctx);
}));

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddOrderStrategies();

builder.Services.AddHttpClient<IDairyClient, DairyClient>();
builder.Services.Configure<DairyClientSettings>(builder.Configuration.GetSection("DairyService"));

builder.Services.AddHttpClient<IProduceClient, ProduceClient>();
builder.Services.Configure<ProduceClientSettings>(builder.Configuration.GetSection("ProduceService"));

builder.Services.AddHttpClient<IDeliveryClient, DeliveryClient>();
builder.Services.Configure<DeliveryClientSettings>(builder.Configuration.GetSection("DeliveryService"));

builder.Services.AddHttpClient<INotificationClient, NotificationClient>();
builder.Services.Configure<NotificationClientSettings>(builder.Configuration.GetSection("NotificationService"));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
