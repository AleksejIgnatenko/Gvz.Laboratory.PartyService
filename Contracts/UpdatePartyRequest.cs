namespace Gvz.Laboratory.PartyService.Contracts
{
    public record UpdatePartyRequest(
        Guid Id,
        int BatchNumber,
        DateTime DateOfReceipt,
        Guid ProductId,
        Guid SupplierId,
        Guid ManufacturerId,
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
        Guid UserId
        );
}
