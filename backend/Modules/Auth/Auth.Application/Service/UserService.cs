using Auth.Application.DTO;
using Auth.Application.Repository;
using Auth.Domain.Data.Model;
using Auth.Domain.Data.Enum;
using Auth.Application.DTO.Response;
using Auth.Application.DTO.Request;
using Auth.Application.Interface;
namespace Auth.Application.Service;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;

    public UserService(IUserRepository userRepository, IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _userRepository.GetByIdAsync(id, ct);
    }

    public async Task<User?> RegisterUserAsync(UserDto user)
    {
        if (await _userRepository.ExistsByEmailAsync(user.Email))
        {
            throw new InvalidOperationException("Email already in use.");
        }

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Name = user.Name,
            Email = user.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password),
            Role = Enum.Parse<UserRole>(user.Role, true)
        };

        await _userRepository.AddAsync(newUser);
        await _userRepository.SaveChangesAsync();

        return newUser;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken ct = default)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, ct);

        if (user is null || string.IsNullOrWhiteSpace(user.PasswordHash) || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        LoginResponseDto response = new LoginResponseDto
        {
            AccessToken = _jwtTokenService.GenerateAccessToken(user),
            Expiration = _jwtTokenService.GetExpirationUtc(),
            RefreshToken = _jwtTokenService.GenerateRefreshToken(),
            UserRole = user.Role.ToString(),
            UserId = user.Id
        };

        return response;
    }
    public async Task<LoginResponseDto> LoginOrRegisterGoogleAsync(
           string email,
           string name,
           string providerKey,
           CancellationToken ct = default)
    {
        _ = providerKey; // keep for future provider-link table (AspNetUserLogins/custom table)

        var user = await _userRepository.GetByEmailAsync(email, ct);

        if (user is null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                UserName = email,
                EmailConfirmed = true,
                Role = default,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString("N"))
            };

            await _userRepository.AddAsync(user, ct);
            await _userRepository.SaveChangesAsync(ct);
        }

        return new LoginResponseDto
        {
            AccessToken = _jwtTokenService.GenerateAccessToken(user),
            Expiration = _jwtTokenService.GetExpirationUtc(),
            RefreshToken = _jwtTokenService.GenerateRefreshToken(),
            UserRole = user.Role.ToString(),
            UserId = user.Id
        };
    }
}
