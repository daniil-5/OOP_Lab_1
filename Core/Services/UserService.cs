// using OOP_Lab_1.Core.Entities;
// using OOP_Lab_1.Core.Entities.Repositories;
// using OOP_Lab_1.Core.Interfaces;
// using Serilog;
//
// namespace OOP_Lab_1.Core.Services;
//
// public class UserService : IUserService
// {
//     private readonly IUserRepository _userRepository;
//
//     // Constructor: accepts the ILoanRepository to handle data access
//     public UserService(IUserRepository userRepository)
//     {
//         _userRepository = userRepository;
//     }
//
//     // Register a new user
//     public async Task<bool> RegisterUserAsync(User user, string bankId)
//     {
//         // Validate input parameters
//         if (user == null)
//             throw new ArgumentNullException(nameof(user));
//
//         if (string.IsNullOrEmpty(bankId))
//             throw new ArgumentException("Bank ID cannot be null or empty.", nameof(bankId));
//
//         // Check if the email is already registered
//         var existingUser = await _userRepository.LoginAsync(user.Email, user.Password, bankId);
//         if (existingUser != null)
//         {
//             throw new InvalidOperationException("Email is already registered.");
//         }
//
//         // Register the user
//         return await _userRepository.RegisterUserAsync(user, bankId);
//     }
//
//     // Login a user
//     public async Task<User> LoginAsync(string email, string password, string bankId)
//     {
//         // Validate input parameters
//         if (string.IsNullOrEmpty(email))
//             throw new ArgumentException("Email cannot be null or empty.", nameof(email));
//
//         if (string.IsNullOrEmpty(password))
//             throw new ArgumentException("Password cannot be null or empty.", nameof(password));
//
//         // Attempt to log in
//         var user = await _userRepository.LoginAsync(email, password, bankId);
//
//         return user;
//     }
//
//     // Get all users of a specific bank
//     public async Task<List<User>> GetUsersByBankAsync(string bankId)
//     {
//         // Validate input parameters
//         if (string.IsNullOrEmpty(bankId))
//             throw new ArgumentException("Bank ID cannot be null or empty.", nameof(bankId));
//
//         // Fetch users by bank
//         return await _userRepository.GetUsersByBankAsync(bankId);
//     }
//
//     // Update user details
//     public async Task<bool> UpdateUserAsync(User user, string bankId)
//     {
//         // Validate input parameters
//         if (user == null)
//             throw new ArgumentNullException(nameof(user));
//
//         if (string.IsNullOrEmpty(bankId))
//             throw new ArgumentException("Bank ID cannot be null or empty.", nameof(bankId));
//
//         // Update the user
//         return await _userRepository.UpdateUserAsync(user, bankId);
//     }
//
//     // Delete a user
//     public async Task<bool> DeleteUserAsync(int userId)
//     {
//         // Validate input parameters
//         if (userId <= 0)
//             throw new ArgumentException("User ID must be greater than zero.", nameof(userId));
//
//         // Delete the user
//         return await _userRepository.DeleteUserAsync(userId);
//     }
//     
//     // Checks if exist the user with such email in current bank
//     
//     public async Task<User> UserExistsByEmailAsync(string email, string bankId)
//     {
//         if (string.IsNullOrEmpty(bankId))
//             throw new ArgumentException("Bank ID cannot be null or empty.", nameof(bankId));
//         
//         if (string.IsNullOrEmpty(email))
//             throw new ArgumentException("User email null or empty.", nameof(email));
//         
//         return await _userRepository.UserExistsByEmailAsync(email, bankId);;
//     }
//
//     public async Task<bool> ApproveUserStatusAsync(User user, string bankId)
//     {
//         // Validate input parameters
//         if (user == null)
//             throw new ArgumentNullException(nameof(user));
//
//         if (string.IsNullOrEmpty(bankId))
//             throw new ArgumentException("Bank ID cannot be null or empty.", nameof(bankId));
//
//         // Check if the email is already registered
//         var existingUser = await _userRepository.LoginAsync(user.Email, user.Password, bankId);
//         if (existingUser != null)
//         {
//             throw new InvalidOperationException("Email is already registered.");
//         }
//
//         // Approve the user
//         return await _userRepository.ApproveUserStatusAsync(user.Id, bankId);
//     }
//     
//     public async Task<List<User>> GetPendingUsersAsync(string bankId)
//     {
//         if (string.IsNullOrEmpty(bankId))
//             throw new ArgumentException("Bank ID cannot be null or empty.", nameof(bankId));
//         
//         // return the list of pending users
//         
//         return await _userRepository.GetPendingUsersAsync(bankId);
//     }
//
//     public async Task<bool> AddExternalSpecialistAsync(int userId, int enterpriseId)
//     {
//         if (userId <= 0)
//             throw new ArgumentException("User id cant be null or empty.", nameof(userId));
//         if (enterpriseId <= 0)
//             throw new ArgumentException("Enterprise ID cannot be null or empty.", nameof(enterpriseId));
//         
//         return await _userRepository.AddExternalSpecialistAsync(userId, enterpriseId);
//     }
//
//     public async Task<int> GetEnterpriseIdByUserIdAsync(int userId)
//     {
//         if (userId <= 0)
//             throw new ArgumentException("User id cant be null or empty.", nameof(userId));
//         
//         return await _userRepository.GetEnterpriseIdByUserIdAsync(userId);
//     }
//     
//     public async Task<List<Enterprise>> GetEnterprisesAsync(string bankId)
//     {
//         if (string.IsNullOrEmpty(bankId))
//             throw new ArgumentException("Bank ID cannot be null or empty.", nameof(bankId));
//         
//         return await _userRepository.GetEnterprisesAsync(bankId);
//     }
//
//
//     public async Task<Enterprise> GetEnterpriseByIdAsync(int enterpriseId)
//     {
//         if (enterpriseId <= 0)
//             throw new ArgumentException("Enterprise ID cannot be null or empty.", nameof(enterpriseId));
//         
//         return await _userRepository.GetEnterpriseByIdAsync(enterpriseId);
//     }
//
//     public async Task<int> GetWorkerIdFromUserAsync(User user)
//     {
//         return await _userRepository.GetWorkerIdFromUserAsync(user);
//     }
//
//     public async Task<int> GetEnterpriseIdByWorkerIdAsync(int workerId)
//     {
//         if (workerId <= 0)
//             throw new ArgumentException("Worker ID cannot be null or empty.", nameof(workerId));
//         return await _userRepository.GetEnterpriseIdByWorkerIdAsync(workerId);
//     }
//
// }
using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Entities.Repositories;
using OOP_Lab_1.Core.Interfaces;
using Serilog;

namespace OOP_Lab_1.Core.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger _logger;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _logger = Log.ForContext<UserService>();
    }

    public async Task<bool> RegisterUserAsync(User user, string bankId)
    {
        _logger.Information("Registering user with email: {Email}, Bank ID: {BankId}", user.Email, bankId);
        
        if (user == null)
            throw new ArgumentNullException(nameof(user));
        if (string.IsNullOrEmpty(bankId))
            throw new ArgumentException("Bank ID cannot be null or empty.", nameof(bankId));
        
        var existingUser = await _userRepository.LoginAsync(user.Email, user.Password, bankId);
        if (existingUser != null)
        {
            _logger.Warning("Registration failed: Email {Email} is already registered.", user.Email);
            throw new InvalidOperationException("Email is already registered.");
        }
        
        var result = await _userRepository.RegisterUserAsync(user, bankId);
        _logger.Information("User registration {Result} for email: {Email}", result ? "successful" : "failed", user.Email);
        return result;
    }

    public async Task<User> LoginAsync(string email, string password, string bankId)
    {
        _logger.Information("Attempting login for email: {Email}, Bank ID: {BankId}", email, bankId);
        if (string.IsNullOrEmpty(email))
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));
        
        var user = await _userRepository.LoginAsync(email, password, bankId);
        _logger.Information("Login {Result} for email: {Email}", user != null ? "successful" : "failed", email);
        return user;
    }

    public async Task<List<User>> GetUsersByBankAsync(string bankId)
    {
        _logger.Information("Fetching users for Bank ID: {BankId}", bankId);
        if (string.IsNullOrEmpty(bankId))
            throw new ArgumentException("Bank ID cannot be null or empty.", nameof(bankId));
        
        return await _userRepository.GetUsersByBankAsync(bankId);
    }

    public async Task<bool> UpdateUserAsync(User user, string bankId)
    {
        _logger.Information("Updating user with ID: {UserId}, Bank ID: {BankId}", user.Id, bankId);
        if (user == null)
            throw new ArgumentNullException(nameof(user));
        if (string.IsNullOrEmpty(bankId))
            throw new ArgumentException("Bank ID cannot be null or empty.", nameof(bankId));
        
        return await _userRepository.UpdateUserAsync(user, bankId);
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        _logger.Information("Deleting user with ID: {UserId}", userId);
        if (userId <= 0)
            throw new ArgumentException("User ID must be greater than zero.", nameof(userId));
        
        return await _userRepository.DeleteUserAsync(userId);
    }

    public async Task<User> UserExistsByEmailAsync(string email, string bankId)
    {
        _logger.Information("Checking if user exists with email: {Email}, Bank ID: {BankId}", email, bankId);
        return await _userRepository.UserExistsByEmailAsync(email, bankId);
    }

    public async Task<bool> ApproveUserStatusAsync(User user, string bankId)
    {
        _logger.Information("Approving user status for email: {Email}, Bank ID: {BankId}", user.Email, bankId);
        return await _userRepository.ApproveUserStatusAsync(user.Id, bankId);
    }
    
    public async Task<List<User>> GetPendingUsersAsync(string bankId)
    {
        _logger.Information("Fetching pending users for Bank ID: {BankId}", bankId);
        return await _userRepository.GetPendingUsersAsync(bankId);
    }

    public async Task<bool> AddExternalSpecialistAsync(int userId, int enterpriseId)
    {
        _logger.Information("Adding external specialist User ID: {UserId} to Enterprise ID: {EnterpriseId}", userId, enterpriseId);
        return await _userRepository.AddExternalSpecialistAsync(userId, enterpriseId);
    }

    public async Task<int> GetEnterpriseIdByUserIdAsync(int userId)
    {
        _logger.Information("Getting Enterprise ID for User ID: {UserId}", userId);
        return await _userRepository.GetEnterpriseIdByUserIdAsync(userId);
    }
    
    public async Task<List<Enterprise>> GetEnterprisesAsync(string bankId)
    {
        _logger.Information("Fetching enterprises for Bank ID: {BankId}", bankId);
        return await _userRepository.GetEnterprisesAsync(bankId);
    }

    public async Task<Enterprise> GetEnterpriseByIdAsync(int enterpriseId)
    {
        _logger.Information("Fetching enterprise details for Enterprise ID: {EnterpriseId}", enterpriseId);
        return await _userRepository.GetEnterpriseByIdAsync(enterpriseId);
    }

    public async Task<int> GetWorkerIdFromUserAsync(User user)
    {
        _logger.Information("Getting Worker ID from User ID: {UserId}", user.Id);
        return await _userRepository.GetWorkerIdFromUserAsync(user);
    }

    public async Task<int> GetEnterpriseIdByWorkerIdAsync(int workerId)
    {
        _logger.Information("Getting Enterprise ID for Worker ID: {WorkerId}", workerId);
        return await _userRepository.GetEnterpriseIdByWorkerIdAsync(workerId);
    }

    public async Task<int> IsConnectedToSalaryAsync(int workerId)
    {
        _logger.Information("IsConnectedToSalary for Worker ID: {WorkerId}", workerId);
        return await _userRepository.IsConnectedToSalaryAsync(workerId);
    }

    public async Task<int> ConnectToSalaryAsync(int workerId)
    {
        _logger.Information("ConnectToSalary for Worker ID: {WorkerId}", workerId);
        return await _userRepository.ConnectToSalaryAsync(workerId);
    }
    
    public async Task<int> DisconnectFromSalaryAsync(int workerId)
    {
        _logger.Information("DisconnectFromSalary for Worker ID: {WorkerId}", workerId);
        return await _userRepository.DisconnectFromSalaryAsync(workerId);
    }

    public async Task<List<Worker>> GetConnectedToSalaryWorkersAsync()
    {
        _logger.Information("Getting Connected to Salary Project Workers");
        return await _userRepository.GetConnectedToSalaryWorkersAsync();
    }
}
