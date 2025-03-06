using OOP_Lab_1.Core.Entities;

namespace OOP_Lab_1.Core.Interfaces;

public interface IUserService
{
    Task<bool> RegisterUserAsync(User user, string bankId);

    Task<User> LoginAsync(string email, string password, string bankId);

    Task<List<User>> GetUsersByBankAsync(string bankId);

    Task<bool> UpdateUserAsync(User user, string bankId);

    Task<bool> DeleteUserAsync(int userId);

    Task<bool> UserExistsByEmailAsync(string email, string bankId);
    
    Task<bool> ApproveUserStatusAsync(User user, string bankId);

    Task<List<User>> GetPendingUsersAsync(string bankId);
}