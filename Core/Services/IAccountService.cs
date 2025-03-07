using OOP_Lab_1.Core.Entities;

namespace OOP_Lab_1.Core.Interfaces;

public interface IAccountService
{
    Task<List<UserAccount>> GetAccountsByEmailAsync(string email, string bankId);
    
    Task<List<UserAccount>> GetAllAccountsAsync();
    
    Task<UserAccount> GetAccountAsync(string accountNumber);
   
    Task<bool> DeleteAccountAsync(string accountNumber);
    
    Task<bool> AddAccountAsync(UserAccount account);
    
    Task<bool> WithdrawAsync(string accountId, double amount);

    Task<bool> RefillAsync(string accountId, double amount);
    
    Task<bool> TransferAsync(string fromAccountId, string toAccountId, double amount);
    
    Task<List<Transaction>> GetAllTransactionsAsync();
}