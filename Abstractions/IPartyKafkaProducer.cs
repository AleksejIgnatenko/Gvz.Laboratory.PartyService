using Gvz.Laboratory.PartyService.Dto;

namespace Gvz.Laboratory.PartyService.Abstractions
{
    public interface IPartyKafkaProducer
    {
        Task SendToKafkaAsync(List<Guid> ids, string topic);
        Task SendToKafkaAsync(PartyDto party, string topic);
    }
}