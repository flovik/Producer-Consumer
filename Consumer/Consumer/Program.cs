using Consumer.Interfaces;
using Consumer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConsumerService, ConsumerService>();

builder.Services.AddControllers();

var app = builder.Build();
app.MapControllers();

app.Run();