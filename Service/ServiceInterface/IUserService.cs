using FinalProject.Model;

namespace FinalProject.Service.ServiceInterface;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserById(int userId);
    // Task<Result<bool>> BlockUserAsync(List<int> userIds);
    // Task<Result<bool>> UnblockUserAsync(List<int> userIds);
    // Task<Result<bool>> DeleteUserAsync(List<int> userIds);
}