using OOP_Lab_1.Core.Entities;

namespace OOP_Lab_1.Core.Interfaces;

public interface IBanksService
{
    /// <summary>
    /// Returns all banks from the database using a raw SQL query.
    /// </summary>
    Task<List<(string BankId, string BankName)>> GetBanksAsync();
    
    /// <summary>
    /// Adds a new bank to the database using a raw SQL query.
    /// </summary>
    Task<bool> AddBankAsync(Bank bank);
    
    /// <summary>
    /// Creates a new bank table in the database using a raw SQL query.
    /// </summary>
    Task CreateBankTableAsync(string id);
        
    /// <summary>
    /// Removes the bank from the database using a raw SQL query.
    /// </summary>
    Task<bool> RemoveBankAsync(string bankName);
        
    /// <summary>
    /// Updates the bank in the database using a raw SQL query.
    /// </summary>
    Task<bool> UpdateBankAsync(Bank bank);
    
    Task CreateAccountTableAsync();
    Task CreateLoanTableAsync();
}