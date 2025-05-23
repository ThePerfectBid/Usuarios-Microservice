using MassTransit;
using MongoDB.Bson;
using log4net;

using Usuarios.Domain.Events;

using Usuarios.Infrastructure.Interfaces;

namespace Usuarios.Infrastructure.Consumer
{
    public class UpdateUserConsumer(IServiceProvider serviceProvider, IUserReadRepository userReadRepository) : IConsumer<UserUpdatedEvent>
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly IUserReadRepository _userReadRepository = userReadRepository;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(UpdateUserConsumer));

        public async Task Consume(ConsumeContext<UserUpdatedEvent> @event)
        {
            _logger.Info($"Procesando UserUpdatedEvent para usuario ID {@event.Message.UserId.Value}");

            try
            {
                var message = @event.Message;

                var bsonUser = new BsonDocument
                {
                    { "_id", message.UserId.Value },
                    { "name", message.Name.Value },
                    { "lastName", message.LastName.Value },
                    { "address", message.Address.Value },
                    { "phone", message.Phone.Value }
                };

                await _userReadRepository.UpdateAsync(bsonUser);
                _logger.Info($"Usuario ID {message.UserId.Value} actualizado exitosamente en la base de datos.");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error al procesar UserUpdatedEvent para usuario ID {@event.Message.UserId.Value}", ex);
                throw;
            }
        }
    }
}