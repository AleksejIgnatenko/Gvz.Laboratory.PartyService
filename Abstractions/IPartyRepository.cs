using Gvz.Laboratory.PartyService.Models;

namespace Gvz.Laboratory.PartyService.Abstractions
{
    public interface IPartyRepository
    {
        Task<PartyModel> CreatePartyAsync(PartyModel party, Guid productId, Guid supplierId, Guid manufacturerId, Guid userId);
        Task DeletePartiesAsync(List<Guid> ids);
        Task<(List<PartyModel> parties, int numberParties)> GetPartiesForPageAsync(int pageNumber);
        Task<(List<PartyModel> parties, int numberParties)> SearchPartiesAsync(string searchQuery, int pageNumber);
        Task<List<PartyModel>> GetPartiesAsync();
        Task<PartyModel> UpdatePartyAsync(PartyModel party, Guid productId, Guid supplierId, Guid manufacturerId);
    }
}