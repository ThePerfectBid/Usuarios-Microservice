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
using Usuarios.Application.Events;
using Usuarios.Domain.Events.EventHandler;
using Usuarios.Infrastructure.Persistence.Repository;
using Usuarios.Infrastructure.Interfaces;
using Usuarios.Infrastructure.Consumer;
using Usuarios.Infrastructure.Persistence.Repository.MongoRead;
using Usuarios.Infrastructure.Queries.QueryHandlers;


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
builder.Services.AddSingleton<MongoReadUserActivityDbConfig>();

// REGISTRA EL REPOSITORIO ANTES DE MediatR
builder.Services.AddScoped<IUserRepository, MongoWriteUserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleWriteRepository>();
builder.Services.AddScoped<IUserReadRepository, MongoReadUserRepository>();
builder.Services.AddScoped<IRoleReadRepository, RoleReadRepository>();
builder.Services.AddScoped<IUserActivityReadRepository, MongoReadUserActivityRepository>();
// REGISTRA MediatR PARA TODOS LOS HANDLERS
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateUserCommandHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateUserCommandHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UserCreatedEventHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UserUpdatedEventHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UserActivityQueryHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetUserByEmailQueryHandler).Assembly));




builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.AddConsumer<CreateUserConsumer>();
    busConfigurator.AddConsumer<UpdateUserConsumer>();
    busConfigurator.AddConsumer<UserActivityConsumer>();
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
        configurator.ReceiveEndpoint(Environment.GetEnvironmentVariable("RABBIT_QUEUE_UPDATE"), e => {
            e.ConfigureConsumer<UpdateUserConsumer>(context);
        });
        configurator.ReceiveEndpoint(Environment.GetEnvironmentVariable("RABBIT_QUEUE_ACTIVITY"), e => {
            e.ConfigureConsumer<UserActivityConsumer>(context);
        });

        configurator.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
        configurator.ConfigureEndpoints(context);
    });
});
EndpointConvention.Map<UserCreatedEvent>(new Uri("queue:" + Environment.GetEnvironmentVariable("RABBIT_QUEUE")));
EndpointConvention.Map<UserUpdatedEvent>(new Uri("queue:" + Environment.GetEnvironmentVariable("RABBIT_QUEUE_UPDATE")));
EndpointConvention.Map<UserActivityMadeEvent>(new Uri("queue:" + Environment.GetEnvironmentVariable("RABBIT_QUEUE_ACTIVITY")));


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
