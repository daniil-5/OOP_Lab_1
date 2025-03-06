using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Entities.Repositories;
using OOP_Lab_1.Core.Interfaces;

namespace OOP_Lab_1.Core.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    // Constructor: accepts the ILoanRepository to handle data access
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // Register a new user
    public async Task<bool> RegisterUserAsync(User user, string bankId)
    {
        // Validate input parameters
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (string.IsNullOrEmpty(bankId))
            throw new ArgumentException("Bank ID cannot be null or empty.", nameof(bankId));

        // Check if the email is already registered
        var existingUser = await _userRepository.LoginAsync(user.Email, user.Password, bankId);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Email is already registered.");
        }

        // Register the user
        return await _userRepository.RegisterUserAsync(user, bankId);
    }

    // Login a user
    public async Task<User> LoginAsync(string email, string password, string bankId)
    {
        // Validate input parameters
        if (string.IsNullOrEmpty(email))
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));

        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));

        // Attempt to log in
        var user = await _userRepository.LoginAsync(email, password, bankId);

        return user;
    }

    // Get all users of a specific bank
    public async Task<List<User>> GetUsersByBankAsync(string bankId)
    {
        // Validate input parameters
        if (string.IsNullOrEmpty(bankId))
            throw new ArgumentException("Bank ID cannot be null or empty.", nameof(bankId));

        // Fetch users by bank
        return await _userRepository.GetUsersByBankAsync(bankId);
    }

    // Update user details
    public async Task<bool> UpdateUserAsync(User user, string bankId)
    {
        // Validate input parameters
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (string.IsNullOrEmpty(bankId))
            throw new ArgumentException("Bank ID cannot be null or empty.", nameof(bankId));

        // Update the user
        return await _userRepository.UpdateUserAsync(user, bankId);
    }

    // Delete a user
    public async Task<bool> DeleteUserAsync(int userId)
    {
        // Validate input parameters
        if (userId <= 0)
            throw new ArgumentException("User ID must be greater than zero.", nameof(userId));

        // Delete the user
        return await _userRepository.DeleteUserAsync(userId);
    }
    
    // Checks if exist the user with such email in current bank
    
    public async Task<bool> UserExistsByEmailAsync(string email, string bankId)
    {
        if (string.IsNullOrEmpty(bankId))
            throw new ArgumentException("Bank ID cannot be null or empty.", nameof(bankId));
        
        if (string.IsNullOrEmpty(email))
            throw new ArgumentException("User email null or empty.", nameof(email));
        
        return await _userRepository.UserExistsByEmailAsync(email, bankId);;
    }

    public async Task<bool> ApproveUserStatusAsync(User user, string bankId)
    {
        // Validate input parameters
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (string.IsNullOrEmpty(bankId))
            throw new ArgumentException("Bank ID cannot be null or empty.", nameof(bankId));

        // Check if the email is already registered
        var existingUser = await _userRepository.LoginAsync(user.Email, user.Password, bankId);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Email is already registered.");
        }

        // Approve the user
        return await _userRepository.ApproveUserStatusAsync(user.Id, bankId);
    }
    
    public async Task<List<User>> GetPendingUsersAsync(string bankId)
    {
        if (string.IsNullOrEmpty(bankId))
            throw new ArgumentException("Bank ID cannot be null or empty.", nameof(bankId));
        
        // return the list of pending users
        
        return await _userRepository.GetPendingUsersAsync(bankId);
    }

}