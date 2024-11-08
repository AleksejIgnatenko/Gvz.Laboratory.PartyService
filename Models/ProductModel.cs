namespace Gvz.Laboratory.PartyService.Models
{
    public class ProductModel
    {
        public Guid Id { get; }
        public string ProductName { get; } = string.Empty;

        public ProductModel(Guid id, string productName)
        {
            Id = id;
            ProductName = productName;
        }

        public ProductModel()
        {
        }

        public static ProductModel Create(Guid id, string productName)
        {
            ProductModel product = new ProductModel(id, productName);
            return product;
        }
    }
}
