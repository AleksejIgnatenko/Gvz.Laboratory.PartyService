using Gvz.Laboratory.PartyService.Dto;
using Gvz.Laboratory.PartyService.Entities;

namespace Gvz.Laboratory.PartyService.Abstractions
{
    public interface ISupplierRepository
    {
        Task<Guid> CreateSupplierAsync(SupplierDto supplier);
        Task DeleteSuppliersAsync(List<Guid> ids);
        Task<SupplierEntity?> GetSupplierByIdAsync(Guid supplierId);
        Task<Guid> UpdateSupplierAsync(SupplierDto supplier);
    }
}