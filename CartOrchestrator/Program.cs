using CartOrchestrator.Clients.DairyClient;
using CartOrchestrator.Clients.ProduceClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpClient<IDairyClient, DairyClient>();
builder.Services.Configure<DairyClientSettings>(builder.Configuration.GetSection("DairyService"));

builder.Services.AddHttpClient<IProduceClient, ProduceClient>();
builder.Services.Configure<ProduceClientSettings>(builder.Configuration.GetSection("ProduceService"));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
