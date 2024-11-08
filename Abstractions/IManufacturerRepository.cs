using Gvz.Laboratory.PartyService.Dto;
using Gvz.Laboratory.PartyService.Entities;

namespace Gvz.Laboratory.PartyService.Abstractions
{
    public interface IManufacturerRepository
    {
        Task<Guid> CreateManufacturerAsync(ManufacturerDto manufacturer);
        Task DeleteManufacturersAsync(List<Guid> ids);
        Task<ManufacturerEntity?> GetManufacturerByIdAsync(Guid manufacturerId);
        Task<Guid> UpdateManufacturerAsync(ManufacturerDto manufacturer);
    }
}