using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AdminPanel.Config;
using FinalProject.Data;
using FinalProject.Model;
using FinalProject.Model.Dto;
using FinalProject.Service.ServiceInterface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FinalProject.Service;

public class AuthService : IAuthService
{
    private readonly DataContext _context;
    private readonly JwtSettings _jwtSettings;

    public AuthService(DataContext context, IOptions<JwtSettings> jwtSettings)
    {
        _context = context;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<Result<User>> CreateUserAsync(UserDto userDto)
    {
        try
        {
            User user = MapDto(userDto);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Result<User>.Success(user);
        }
        catch (DbUpdateException ex)
        {
            return Result<User>.Fail("A user with the same email already exists.");
        }
    }


    public async Task<Result<User?>> AuthenticateAsync(LogInDto logInDto)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == logInDto.Email);
        await _context.SaveChangesAsync();
        return Result<User?>.Success(user);
    }


    public string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
        var claims = PopulateArr(user);
        return GetTokenHandler(claims, securityKey, credentials);
    }
    
    private Claim[] PopulateArr(User user)
    {
        return new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
    }

    private string GetTokenHandler(Claim[] claims, SymmetricSecurityKey securityKey, SigningCredentials credentials)
    {
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_jwtSettings.ExpirationInMinutes)),
            signingCredentials: credentials
        );
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private User MapDto(UserDto userDto)
    {
        var user = new User
        {
            UserName = userDto.UserName,
            Name = userDto.Name,
            Email = userDto.Email,
            Role = Role.User,
        };
        return user;
    }
}