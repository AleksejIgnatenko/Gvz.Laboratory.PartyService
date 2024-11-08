using Gvz.Laboratory.PartyService.Abstractions;
using Gvz.Laboratory.PartyService.Dto;
using Gvz.Laboratory.PartyService.Entities;
using Gvz.Laboratory.PartyService.Models;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.PartyService.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly GvzLaboratoryPartyServiceDbContext _context;

        public ProductRepository(GvzLaboratoryPartyServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateProductAsync(ProductDto product)
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(s => s.ProductName.Equals(product.ProductName));

            if (existingProduct == null)
            {
                var productEntity = new ProductEntity
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                };

                await _context.Products.AddAsync(productEntity);
                await _context.SaveChangesAsync();
            }

            return product.Id;
        }

        public async Task<ProductEntity?> GetProductByIdAsync(Guid productId)
        {
            var productEntities = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId);

            return productEntities;
        }

        public async Task<Guid> UpdateProductAsync(ProductDto product)
        {
            await _context.Products
                .Where(p => p.Id == product.Id)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(p => p.ProductName, product.ProductName));

            return product.Id;
        }

        public async Task DeleteProductsAsync(List<Guid> ids)
        {
            var productEntities = await _context.Products
                .Include(p => p.Parties)
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();

            foreach (var productEntity in productEntities)
            {
                productEntity.Parties.Clear();
            }

            await _context.SaveChangesAsync();

            await _context.Products
                .Where(s => ids.Contains(s.Id))
                .ExecuteDeleteAsync();
        }
    }
}
