using FinalProject.Model;
using FinalProject.Model.Dto;

namespace FinalProject.Service.ServiceInterface;

public interface IAuthService
{
    Task<Result<User>> CreateUserAsync(UserDto userDto);
    Task<Result<User?>> AuthenticateAsync(LogInDto logInDto);
    string? GenerateJwtToken(User user);
}