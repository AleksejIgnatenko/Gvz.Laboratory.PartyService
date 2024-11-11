using Gvz.Laboratory.PartyService.Abstractions;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace Gvz.Laboratory.PartyService.Infrastructure
{
    public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
    {
        private readonly JwtOptions _options = options.Value;

        public Guid GetUserIdFromToken(string jwtToken)
        {
            // Разбор JWT-токена для извлечения идентификатора пользователя
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(jwtToken);
            var userId = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception();
            }

            return Guid.Parse(userId);
        }
    }
}
