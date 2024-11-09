using Confluent.Kafka;
using Gvz.Laboratory.PartyService.Abstractions;
using Gvz.Laboratory.PartyService.Dto;
using Serilog;
using System.Text.Json;

namespace Gvz.Laboratory.PartyService.Kafka
{
    public class UpdateUserKafkaConsumer : IHostedService
    {
        private readonly ConsumerConfig _config;
        private IConsumer<Ignore, string> _consumer;
        private CancellationTokenSource _cts;
        private readonly IUserRepository _userRepository;

        public UpdateUserKafkaConsumer(ConsumerConfig config, IUserRepository userRepository)
        {
            _config = config;
            _userRepository = userRepository;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _consumer = new ConsumerBuilder<Ignore, string>(_config).Build();

            _consumer.Subscribe("update-user-topic");

            Task.Run(() => ConsumeMessages(cancellationToken));

            return Task.CompletedTask;
        }

        private async void ConsumeMessages(CancellationToken cancellationToken)
        {
            try
            {
                while (true)
                {
                    try
                    {
                        var cr = _consumer.Consume(cancellationToken);

                        var updateUserDto = JsonSerializer.Deserialize<UserDto>(cr.Message.Value)
                            ?? throw new InvalidOperationException("Deserialization failed: UserDto is null.");

                        var addManufacturerId = await _userRepository.UpdateUserAsync(updateUserDto);

                    }
                    catch (ConsumeException e)
                    {
                        Log.Error($"Error occurred: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _consumer.Close();
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex.Message);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
