using FinalProject.Model;
using FinalProject.Model.Dto;

namespace FinalProject.Service;

public interface IAuthService
{
    Task<Result<User>> CreateUserAsync(UserDto userDto);
    Task<Result<User?>> AuthenticateAsync(LogInDto logInDto);
    string? GenerateJwtToken(User user);
}