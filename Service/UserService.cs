using AdminPanel.Config;
using FinalProject.Data;
using FinalProject.Model;
using FinalProject.Service.ServiceInterface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FinalProject.Service;

public class UserService : IUserService
{
    private readonly DataContext _context;

    public UserService(DataContext context, IOptions<JwtSettings> jwtSettings)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetUserById(int userId)
    {
        return await _context.Users.SingleOrDefaultAsync(u => u.UserId == userId);
    }
}