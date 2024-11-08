namespace Gvz.Laboratory.PartyService.Contracts
{
    public record GetPartiesResponse(
        Guid Id,
        int BatchNumber,
        DateTime DateOfReceipt,
        string ProductName,
        string SupplierName,
        string ManufacturerName,
        double BatchSize,
        double SampleSize,
        int TTN,
        string DocumentOnQualityAndSafety,
        string TestReport,
        DateTime DateOfManufacture,
        DateTime ExpirationDate,
        string Packaging,
        string Marking,
        string Result,
        string Note,
        string UserName
        );
}
