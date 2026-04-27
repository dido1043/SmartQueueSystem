using System;

namespace Auth.Application.DTO.Response;

public class LoginResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public string UserRole { get; set; } = string.Empty;
    public Guid UserId { get; set; } = Guid.Empty;
    
}
