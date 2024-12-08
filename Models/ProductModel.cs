namespace Gvz.Laboratory.PartyService.Models
{
    public class ProductModel
    {
        public Guid Id { get; }
        public string ProductName { get; } = string.Empty;
        public string UnitsOfMeasurement { get; set; } = string.Empty;

        public ProductModel(Guid id, string productName, string unitsOfMeasurement)
        {
            Id = id;
            ProductName = productName;
            UnitsOfMeasurement = unitsOfMeasurement;
        }

        public ProductModel()
        {
        }

        public static ProductModel Create(Guid id, string productName, string unitsOfMeasurement)
        {
            ProductModel product = new ProductModel(id, productName, unitsOfMeasurement);
            return product;
        }
    }
}
