using Gvz.Laboratory.PartyService.Abstractions;
using Gvz.Laboratory.PartyService.Dto;
using Gvz.Laboratory.PartyService.Entities;
using Gvz.Laboratory.PartyService.Models;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.PartyService.Repositories
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private readonly GvzLaboratoryPartyServiceDbContext _context;

        public ManufacturerRepository(GvzLaboratoryPartyServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateManufacturerAsync(ManufacturerDto manufacturer)
        {
            var existingManufacturer = await _context.Manufacturers.FirstOrDefaultAsync(m => m.ManufacturerName.Equals(manufacturer.ManufacturerName));
            Console.WriteLine("111111111111111111111111111111");
            if (existingManufacturer == null)
            {

                var manufacturerEntity = new ManufacturerEntity
                {
                    Id = manufacturer.Id,
                    ManufacturerName = manufacturer.ManufacturerName,
                };

                await _context.Manufacturers.AddAsync(manufacturerEntity);
                await _context.SaveChangesAsync();
            }

            return manufacturer.Id;
        }

        public async Task<ManufacturerEntity?> GetManufacturerByIdAsync(Guid manufacturerId)
        {
            var manufacturerEntities = await _context.Manufacturers
                .FirstOrDefaultAsync(m => m.Id == manufacturerId);

            return manufacturerEntities;
        }

        public async Task<Guid> UpdateManufacturerAsync(ManufacturerDto manufacturer)
        {
            await _context.Manufacturers
                .Where(m => m.Id == manufacturer.Id)
                .ExecuteUpdateAsync(m => m
                    .SetProperty(m => m.ManufacturerName, manufacturer.ManufacturerName)
                 );

            return manufacturer.Id;
        }

        public async Task DeleteManufacturersAsync(List<Guid> ids)
        {
            await _context.Manufacturers
                .Where(s => ids.Contains(s.Id))
                .ExecuteDeleteAsync();
        }
    }
}
