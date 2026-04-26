using Microsoft.AspNetCore.Mvc;
using Auth.Application.Service;
using Auth.Application.DTO;
using Auth.Application.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;

namespace SmartQueue.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDto userDto)
        {
            try
            {
                return Ok(await _userService.RegisterUserAsync(userDto));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            try
            {
                var response = await _userService.LoginAsync(loginRequest);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GoogleCallback))
            };
            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        [AllowAnonymous]
        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var auth = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!auth.Succeeded || auth.Principal is null) return Unauthorized();
            var email = auth.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var name = auth.Principal.FindFirst(ClaimTypes.Name)?.Value ?? "Google User";
            var sub = auth.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Google unique id

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(sub))
                return BadRequest("Google claims missing.");

            var ct = HttpContext.RequestAborted;
            var result = await _userService.LoginOrRegisterGoogleAsync(email, name, sub, ct);
            return Ok(result); // your JWT response
        }
    }
}
