using OOP_Lab_1.Core.Entities;

namespace OOP_Lab_1.Core.Interfaces;

public interface IAccountService
{
    Task<List<UserAccount>> GetAccountsByEmailAsync(string tableName, string email);
    Task<bool> UpdateAccountAsync(string tableName, string accountNumber, double balance, bool isBlocked, bool isFrozen, string email);
    Task<bool> DeleteAccountAsync(string tableName, string accountNumber);
    Task<bool> AddAccountAsync(string tableName, UserAccount account);
}