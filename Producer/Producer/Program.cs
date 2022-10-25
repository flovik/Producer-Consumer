using Producer.Interfaces;
using Producer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IProducerService, ProducerService>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

Task.Run(() =>
{
    using var serviceScope = app.Services.CreateScope();
    var services = serviceScope.ServiceProvider;
    var _ = services.GetService<IProducerService>();
});

app.Run();
