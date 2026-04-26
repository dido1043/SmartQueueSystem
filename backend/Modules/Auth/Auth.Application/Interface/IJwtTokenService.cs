using Auth.Domain.Data.Model;

namespace Auth.Application.Interface;

public interface IJwtTokenService
{
    string GenerateAccessToken(User user);
    DateTime GetExpirationUtc();
    string GenerateRefreshToken();
}
