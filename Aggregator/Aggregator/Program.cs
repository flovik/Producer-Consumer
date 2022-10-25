using Aggregator.Interfaces;
using Aggregator.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IAggregatorService, AggregatorService>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
