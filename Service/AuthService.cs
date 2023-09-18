using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AdminPanel.Config;
using FinalProject.Data;
using FinalProject.Model;
using FinalProject.Model.Dto;
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

    // private string GenerateVerificationCode()
    // {
    //     const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    //     var random = new Random();
    //     var code = new string(Enumerable.Repeat(chars, 6)
    //         .Select(s => s[random.Next(s.Length)]).ToArray());
    //     return code;
    // }

    public async Task<Result<User>> CreateUserAsync(UserDto userDto)
    {
        try
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);
            if (existingUser != null)
            {
                if (existingUser.RegistrationMethod is "Google" or "GitHub")
                {
                    Result<User>.Fail("User with the same email is registered via social network");
                }
                else
                {
                    return Result<User>.Fail("A user with the same email already exists.");
                }
            }

            if (await _context.Users.AnyAsync(u => u.UserName == userDto.UserName))
            {
                return Result<User>.Fail("A user with the same username already exists.");
            }

            User user = MapDto(userDto);
            // var verificationCode = GenerateVerificationCode();
            // user.VerificationCode = verificationCode;
            _context.Users.Add(user);
            var result = await _context.SaveChangesAsync();
            if (result == 0)
                return Result<User>.Fail("A user with the same email already exists.");
            // await SendVerificationEmail(user.Email, verificationCode);
            return Result<User>.Success(user);
        }
        catch (DbUpdateException ex)
        {
            return Result<User>.Fail("A user with the same email already exists.");
        }
    }
    
    // private async Task SendVerificationEmail(string userEmail, string verificationCode)
    // { }

    public async Task<Result<User?>> AuthenticateAsync(LogInDto logInDto)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == logInDto.Email);
        var valid = CheckValidity(user, logInDto.Password);
        if (valid != null) return valid;
        await _context.SaveChangesAsync();
        return Result<User?>.Success(user);
    }


    public string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_jwtSettings.ExpirationInMinutes)),
            signingCredentials: credentials
        );
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }

    private User MapDto(UserDto userDto)
    {
        var user = new User
        {
            UserName = userDto.UserName,
            Name = userDto.Name,
            PasswordHash = HashPassword(userDto.Password),
            Email = userDto.Email,
            Role = Role.User,
        };
        return user;
    }

    private Result<User?>? CheckValidity(User? user, string password)
    {
        if (user == null || !VerifyPassword(password, user.PasswordHash))
            return Result<User?>.Fail("User's mail or password is incorrect");
        return null;
    }
}