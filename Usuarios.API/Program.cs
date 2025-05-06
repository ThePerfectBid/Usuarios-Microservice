using MediatR;
using DotNetEnv;
using Microsoft.Extensions.DependencyInjection;
using Usuarios.Application.Handlers; // Asegúrate de importar el namespace correcto
using Usuarios.Infrastructure.Persistence;
using Usuarios.Domain.Repositories;
using MongoDB.Driver;
using Usuarios.Infrastructure.Persistence.Repository.MongoWrite;
using Usuarios.Infrastructure.Configurations;
using Usuarios.Application.EventHandlers;
using Usuarios.Domain.Events;
using MassTransit;
using Application.Core;
using Usuarios.Infrastructure.Persistence.Repository;
using Usuarios.Infrastructure.Interfaces;



var builder = WebApplication.CreateBuilder(args);

Env.Load();
// Add services to the container.

// 🔹 Suscribir el manejador de eventos al dispatcher
//eventDispatcher.Subscribe(e => new UserCreatedEventHandler().Handle((UserCreatedEvent)e));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar configuración de MongoDB
builder.Services.AddSingleton<MongoWriteDbConfig>();
builder.Services.AddSingleton<MongoReadDbConfig>();

// REGISTRA EL REPOSITORIO ANTES DE MediatR
builder.Services.AddScoped<IUserRepository, MongoWriteUserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleWriteRepository>();
builder.Services.AddScoped<IUserReadRepository, MongoReadUserRepository>();
builder.Services.AddScoped<IRoleReadRepository, RoleReadRepository>();

// REGISTRA MediatR PARA TODOS LOS HANDLERS
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateUserHandler).Assembly));



    builder.Services.AddMassTransit(busConfigurator =>
    {
        busConfigurator.AddConsumer<CreateUserConsumer>();
        busConfigurator.SetKebabCaseEndpointNameFormatter();
        busConfigurator.UsingRabbitMq((context, configurator) =>
        {
            configurator.Host(new Uri(Environment.GetEnvironmentVariable("RABBIT_URL")), h =>
            {
                h.Username(Environment.GetEnvironmentVariable("RABBIT_USERNAME"));
                h.Password(Environment.GetEnvironmentVariable("RABBIT_PASSWORD"));
            });

            configurator.ReceiveEndpoint(Environment.GetEnvironmentVariable("RABBIT_QUEUE"), e => {
                e.ConfigureConsumer<CreateUserConsumer>(context); 
            });

            configurator.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
            configurator.ConfigureEndpoints(context);
        });
    });
    EndpointConvention.Map<UserCreatedEvent>(new Uri("queue:user-queue"));


var app = builder.Build();

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
