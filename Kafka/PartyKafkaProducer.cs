using Confluent.Kafka;
using Gvz.Laboratory.PartyService.Abstractions;
using Gvz.Laboratory.PartyService.Dto;
using System.Text.Json;

namespace Gvz.Laboratory.PartyService.Kafka
{
    public class PartyKafkaProducer : IPartyKafkaProducer
    {
        private readonly IProducer<Null, string> _producer;

        public PartyKafkaProducer(IProducer<Null, string> producer)
        {
            _producer = producer;
        }

        public async Task SendToKafkaAsync(PartyDto party, string topic)
        {
            var serializedParty = JsonSerializer.Serialize(party);
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = serializedParty });
        }

        public async Task SendToKafkaAsync(List<Guid> ids, string topic)
        {
            var serializedId = JsonSerializer.Serialize(ids);
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = serializedId });
        }
    }
}
