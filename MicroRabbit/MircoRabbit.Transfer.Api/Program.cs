
using MediatR;
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.IoC;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Domain.EventHandlers;
using MicroRabbit.Transfer.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




var connectionString = builder.Configuration.GetConnectionString("TransferDbConnection");
var services = builder.Services.AddDbContext<TransferDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

DependencyContainer.RegisterServices(services);

builder.Services.AddMediatR(typeof(Program));


var app = builder.Build();



ServiceProvider serviceProvider = builder.Services.BuildServiceProvider();
var eventBus = serviceProvider.GetRequiredService<IEventBus>();
eventBus.Subscribe<TransferCreatedEvent, TransferEventHandler>();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
