using Gvz.Laboratory.PartyService.Dto;
using Gvz.Laboratory.PartyService.Entities;

namespace Gvz.Laboratory.PartyService.Abstractions
{
    public interface IProductRepository
    {
        Task<Guid> CreateProductAsync(ProductDto product);
        Task DeleteProductsAsync(List<Guid> ids);
        Task<ProductEntity?> GetProductByIdAsync(Guid productId);
        Task<Guid> UpdateProductAsync(ProductDto product);
    }
}