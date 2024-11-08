using Gvz.Laboratory.PartyService.Abstractions;
using Gvz.Laboratory.PartyService.Dto;
using Gvz.Laboratory.PartyService.Entities;
using Gvz.Laboratory.PartyService.Models;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.PartyService.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly GvzLaboratoryPartyServiceDbContext _context;

        public SupplierRepository(GvzLaboratoryPartyServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateSupplierAsync(SupplierDto supplier)
        {
            var existingSupplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.SupplierName.Equals(supplier.SupplierName));

            if (existingSupplier == null)
            {
                var supplierEntity = new SupplierEntity
                {
                    Id = supplier.Id,
                    SupplierName = supplier.SupplierName,
                };

                await _context.Suppliers.AddAsync(supplierEntity);
                await _context.SaveChangesAsync();
            }

            return supplier.Id;
        }

        public async Task<SupplierEntity?> GetSupplierByIdAsync(Guid supplierId)
        {
            var supplierEntities = await _context.Suppliers
                .FirstOrDefaultAsync(s => s.Id == supplierId);

            return supplierEntities;
        }

        public async Task<Guid> UpdateSupplierAsync(SupplierDto supplier)
        {
            await _context.Suppliers
                .Where(s => s.Id == supplier.Id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(s => s.SupplierName, supplier.SupplierName)
                 );

            return supplier.Id;
        }

        public async Task DeleteSuppliersAsync(List<Guid> ids)
        {
            var supplierEntities = await _context.Suppliers
                .Include(s => s.Parties)
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();

            foreach (var supplierEntity in supplierEntities)
            {
                supplierEntity.Parties.Clear();
            }

            await _context.SaveChangesAsync();

            await _context.Suppliers
                .Where(s => ids.Contains(s.Id))
                .ExecuteDeleteAsync();
        }
    }
}
