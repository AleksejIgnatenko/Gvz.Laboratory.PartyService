namespace Gvz.Laboratory.PartyService.Entities
{
    public class PartyEntity
    {
        public Guid Id { get; set; }
        public int BatchNumber { get; set; }
        public string DateOfReceipt { get; set; } = string.Empty;
        public ProductEntity Product { get; set; } = new ProductEntity();
        public SupplierEntity Supplier { get; set; } = new SupplierEntity();
        public ManufacturerEntity Manufacturer { get; set; } = new ManufacturerEntity();
        public double BatchSize { get; set; }
        public double SampleSize { get; set; }
        public int TTN { get; set; }
        public string DocumentOnQualityAndSafety { get; set; } = string.Empty;
        public string TestReport { get; set; } = string.Empty;
        public string DateOfManufacture { get; set; } = string.Empty;
        public string ExpirationDate { get; set; } = string.Empty;
        public string Packaging { get; set; } = string.Empty;
        public string Marking { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public UserEntity User { get; set; } = new UserEntity();
        public DateTime DateCreate { get; set; }
    }
}
