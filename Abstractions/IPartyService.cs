using Gvz.Laboratory.PartyService.Models;

namespace Gvz.Laboratory.PartyService.Abstractions
{
    public interface IPartyService
    {
        Task<Guid> CreatePartyAsync(Guid id, int batchNumber, string dateOfReceipt, Guid productId, Guid supplierId, Guid manufacturerId, double batchSize, double sampleSize, int ttn, string documentOnQualityAndSafety, string testReport, string dateOfManufacture, string expirationDate, string packaging, string marking, string result, Guid userId, string note);
        Task DeletePartyAsync(List<Guid> ids);
        Task<(List<PartyModel> parties, int numberParties)> GetPartiesForPageAsync(int pageNumber);
        Task<(List<PartyModel> parties, int numberParties)> SearchPartiesAsync(string searchQuery, int pageNumber);
        Task<MemoryStream> ExportPartiesToExcelAsync();
        Task<Guid> UpdatePartyAsync(Guid id, int batchNumber, string dateOfReceipt, Guid productId, Guid supplierId, Guid manufacturerId, double batchSize, double sampleSize, int ttn, string documentOnQualityAndSafety, string testReport, string dateOfManufacture, string expirationDate, string packaging, string marking, string result, string note);
    }
}