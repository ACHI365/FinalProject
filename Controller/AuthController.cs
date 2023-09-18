using FinalProject.Model.Dto;
using FinalProject.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            Console.WriteLine("At least here");
            var result = await _authService.CreateUserAsync(userDto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(new { errorMessage = result.ErrorMessage });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LogInDto loginDto)
        {
            var result = await _authService.AuthenticateAsync(loginDto);
            if (!result.IsSuccess) return Unauthorized(new { errorMessage = result.ErrorMessage });
            var token = _authService.GenerateJwtToken(result.Data);
            var responseData = new
            {
                Token = token,
                Result = result 
            };
            return Ok(responseData);
        }
        
        [HttpGet("google-login")]
        [AllowAnonymous]
        public IActionResult GoogleLogin()
        {
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            };

            return Challenge(authenticationProperties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            // Handle the response from Google authentication
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Handle the authentication result based on your application's logic
            Console.WriteLine(authenticateResult);

            return Ok(authenticateResult);
        }
        
        // [HttpPost("verify-email")]
        // [AllowAnonymous]
        // public async Task<IActionResult> VerifyEmail([FromBody] EmailVerificationDto emailVerificationDto)
        // {
        //     var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == emailVerificationDto.Email 
        //                                                              && u.VerificationCode == emailVerificationDto.VerificationCode);
        //     if (user == null)
        //         return BadRequest(new { errorMessage = "Invalid verification code or email." });
        //     user.IsEmailVerified = true;
        //     _context.Update(user);
        //     await _context.SaveChangesAsync();
        //
        //     return Ok(new { message = "Email verified successfully." });
        // }
    }
}