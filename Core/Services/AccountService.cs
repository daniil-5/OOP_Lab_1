using Microsoft.Data.Sqlite;
using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;

namespace OOP_Lab_1.Core.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
    }

    // Method to withdraw money from an account
    public async Task<bool> WithdrawAsync(string accountId, double amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

        return await _accountRepository.WithdrawAsync(accountId, amount);
    }
    
    // Method to refill money 
    public async Task<bool> RefillAsync(string accountId, double amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

        return await _accountRepository.RefillAsync(accountId, amount);
    }

    // Get accounts by email
    public async Task<List<UserAccount>> GetAccountsByEmailAsync(string email, string bankId)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.", nameof(email));

        return await _accountRepository.GetAccountsByEmailAsync(email, bankId);
    }
    
    // Get all accounts
    public async Task<List<UserAccount>> GetAllAccountsAsync()
    {
        return await _accountRepository.GetAllAccountsAsync();
    }

    // Add a new account
    public async Task<bool> AddAccountAsync(UserAccount account)
    {
        if (account == null)
            throw new ArgumentNullException(nameof(account));

        return await _accountRepository.AddAccountAsync(account);
    }

    // Delete an account
    public async Task<bool> DeleteAccountAsync(string accountNumber)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
            throw new ArgumentException("Account number cannot be empty.", nameof(accountNumber));

        return await _accountRepository.DeleteAccountAsync(accountNumber);
    }

    // Get a single account by account number
    public async Task<UserAccount> GetAccountAsync(string accountNumber)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
            throw new ArgumentException("Account number cannot be empty.", nameof(accountNumber));

        return await _accountRepository.GetAccountAsync(accountNumber);
    }

    public async Task<bool> TransferAsync(string fromAccountNumber, string toAccountNumber, double amount)
    {
        if (string.IsNullOrWhiteSpace(fromAccountNumber))
            throw new ArgumentException("Account number cannot be empty.", nameof(fromAccountNumber));
        if (string.IsNullOrWhiteSpace(toAccountNumber))
            throw new ArgumentException("Account number cannot be empty.", nameof(toAccountNumber));
        if (amount <= 0.0)
            throw new ArgumentException("Amount of transaction must be positive double value.", nameof(amount));
        
        return await _accountRepository.TransferAsync(fromAccountNumber, toAccountNumber, amount);
    }
    
    // Get all transactions
    public async Task<List<Transaction>> GetAllTransactionsAsync()
    {
        return await _accountRepository.GetAllTransactionsAsync();
    }
}
