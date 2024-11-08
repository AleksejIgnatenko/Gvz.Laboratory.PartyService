namespace Gvz.Laboratory.PartyService.Contracts
{
    public record GetPartiesForPageResponseWrapper(
        List<GetPartiesResponse> Parties,
        int numberParties
        );
}
