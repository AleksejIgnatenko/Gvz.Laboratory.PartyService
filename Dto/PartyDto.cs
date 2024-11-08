namespace Gvz.Laboratory.PartyService.Dto
{
    public class PartyDto
    {
        public Guid Id { get; set; }
        public int BatchNumber { get; set; }
        public DateTime DateOfReceipt { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public Guid SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public Guid ManufacturerId { get; set; }
        public string ManufacturerName { get; set; } = string.Empty;
        public double BatchSize { get; set; }
        public double SampleSize { get; set; }
        public int TTN { get; set; }
        public string DocumentOnQualityAndSafety { get; set; } = string.Empty;
        public string TestReport { get; set; } = string.Empty;
        public DateTime DateOfManufacture { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Packaging { get; set; } = string.Empty;
        public string Marking { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}
