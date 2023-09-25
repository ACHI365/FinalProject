using FinalProject.Model.Dto;
using FinalProject.Service;
using FinalProject.Service.ServiceInterface;
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
            var result = await _authService.CreateUserAsync(userDto);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(new { errorMessage = result.ErrorMessage });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LogInDto loginDto)
        {
            Console.WriteLine("HEY");
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
        
    }
}