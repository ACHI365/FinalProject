using AdminPanel.Config;
using FinalProject.Data;
using FinalProject.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        return await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
    }
    //
    // public async Task<Result<bool>> BlockUserAsync(List<int> userIds)
    // {
    //     foreach (var userId in userIds)
    //     {
    //         var user = await _context.Users.FindAsync(userId);
    //         if (user == null)
    //             return Result<bool>.Fail("User not found.");
    //         user.Block();
    //     }
    //     await _context.SaveChangesAsync();
    //     return Result<bool>.Success(true);
    // }
    //
    // public async Task<Result<bool>> UnblockUserAsync(List<int> userIds)
    // {
    //     foreach (var userId in userIds)
    //     {
    //         var user = await _context.Users.FindAsync(userId);
    //         if (user == null)
    //             return Result<bool>.Fail("User not found.");
    //         user.Unblock();
    //     }
    //     await _context.SaveChangesAsync();
    //     return Result<bool>.Success(true);
    // }
    //
    // public async Task<Result<bool>> DeleteUserAsync(List<int> userIds)
    // {
    //     foreach (var userId in userIds)
    //     {
    //         var user = await _context.Users.FindAsync(userId);
    //         if (user == null)
    //             return Result<bool>.Fail("User not found.");
    //         _context.Users.Remove(user);
    //     }
    //     await _context.SaveChangesAsync();
    //     return Result<bool>.Success(true);
    // }
}