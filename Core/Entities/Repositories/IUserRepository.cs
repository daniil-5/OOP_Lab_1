using OOP_Lab_1.Core.Entities;

namespace OOP_Lab_1.Core.Interfaces;

public interface IUserRepository
{
    Task<bool> RegisterUserAsync(User user, string bankId);

    Task<User> LoginAsync(string email, string password, string bankId);

    Task<List<User>> GetUsersByBankAsync(string bankId);

    Task<bool> UpdateUserAsync(User user, string bankId);

    Task<bool> DeleteUserAsync(int userId);

    Task<User> UserExistsByEmailAsync(string email, string bankId);

    Task<bool> ApproveUserStatusAsync(int userId, string bankId);

    Task<List<User>> GetPendingUsersAsync(string bankId);

    Task<bool> AddExternalSpecialistAsync(int userId, int enterpriseId);

    Task<int> GetEnterpriseIdByUserIdAsync(int userId);

    Task<List<Enterprise>> GetEnterprisesAsync(string bankId);

    Task<Enterprise> GetEnterpriseByIdAsync(int enterpriseId);

    Task<int> GetWorkerIdFromUserAsync(User user);

    Task<int> GetEnterpriseIdByWorkerIdAsync(int workerId);

    Task<int> IsConnectedToSalaryAsync(int workerId);

    Task<int> ConnectToSalaryAsync(int workerId);

    Task<int> DisconnectFromSalaryAsync(int workerId);

    Task<List<Worker>> GetConnectedToSalaryWorkersAsync();

}