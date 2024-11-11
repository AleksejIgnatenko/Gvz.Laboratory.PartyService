namespace Gvz.Laboratory.PartyService.Contracts
{
    public record CreatePartyRequest(
        int BatchNumber,
        string DateOfReceipt,
        Guid ProductId,
        Guid SupplierId,
        Guid ManufacturerId,
        double BatchSize,
        double SampleSize,
        int TTN,
        string DocumentOnQualityAndSafety,
        string TestReport,
        string DateOfManufacture,
        string ExpirationDate,
        string Packaging, 
        string Marking,
        string Result, 
        string Note
    );
}
