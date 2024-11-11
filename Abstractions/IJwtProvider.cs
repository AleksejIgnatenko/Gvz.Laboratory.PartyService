namespace Gvz.Laboratory.PartyService.Abstractions
{
    public interface IJwtProvider
    {
        Guid GetUserIdFromToken(string jwtToken);
    }
}