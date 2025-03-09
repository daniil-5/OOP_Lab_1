using Microsoft.Data.Sqlite;
using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;
using Serilog;

namespace OOP_Lab_1.Core.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ILogger _logger;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _logger = Log.ForContext<AccountService>();
    }

    public async Task<bool> WithdrawAsync(string accountId, double amount)
    {
        if (amount <= 0)
        {
            _logger.Warning("Attempt to withdraw a non-positive amount: {Amount}", amount);
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));
        }

        _logger.Information("Withdrawing {Amount} from account {AccountId}", amount, accountId);
        var result = await _accountRepository.WithdrawAsync(accountId, amount);
        _logger.Information("Withdrawal {Status} for account {AccountId}", result ? "succeeded" : "failed", accountId);
        return result;
    }

    public async Task<bool> RefillAsync(string accountId, double amount)
    {
        if (amount <= 0)
        {
            _logger.Warning("Attempt to refill a non-positive amount: {Amount}", amount);
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));
        }

        _logger.Information("Refilling {Amount} to account {AccountId}", amount, accountId);
        var result = await _accountRepository.RefillAsync(accountId, amount);
        _logger.Information("Refill {Status} for account {AccountId}", result ? "succeeded" : "failed", accountId);
        return result;
    }

    public async Task<List<UserAccount>> GetAccountsByEmailAsync(string email, string bankId)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            _logger.Warning("Attempt to get accounts with empty email.");
            throw new ArgumentException("Email cannot be empty.", nameof(email));
        }

        _logger.Information("Fetching accounts for email {Email} in bank {BankId}", email, bankId);
        return await _accountRepository.GetAccountsByEmailAsync(email, bankId);
    }

    public async Task<List<UserAccount>> GetAllAccountsAsync()
    {
        _logger.Information("Fetching all accounts.");
        return await _accountRepository.GetAllAccountsAsync();
    }

    public async Task<bool> AddAccountAsync(UserAccount account)
    {
        if (account == null)
        {
            _logger.Warning("Attempt to add a null account.");
            throw new ArgumentNullException(nameof(account));
        }

        _logger.Information("Adding new account for user {UserId}", account.UserEmail);
        return await _accountRepository.AddAccountAsync(account);
    }

    public async Task<bool> DeleteAccountAsync(string accountNumber)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
        {
            _logger.Warning("Attempt to delete an account with an empty account number.");
            throw new ArgumentException("Account number cannot be empty.", nameof(accountNumber));
        }

        _logger.Information("Deleting account {AccountNumber}", accountNumber);
        return await _accountRepository.DeleteAccountAsync(accountNumber);
    }

    public async Task<UserAccount> GetAccountAsync(string accountNumber)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
        {
            _logger.Warning("Attempt to get an account with an empty account number.");
            throw new ArgumentException("Account number cannot be empty.", nameof(accountNumber));
        }

        _logger.Information("Fetching account {AccountNumber}", accountNumber);
        return await _accountRepository.GetAccountAsync(accountNumber);
    }

    public async Task<bool> TransferAsync(string fromAccountNumber, string toAccountNumber, double amount)
    {
        if (string.IsNullOrWhiteSpace(fromAccountNumber) || string.IsNullOrWhiteSpace(toAccountNumber) || amount <= 0)
        {
            _logger.Warning("Invalid transfer attempt: from {From}, to {To}, amount {Amount}", fromAccountNumber, toAccountNumber, amount);
            throw new ArgumentException("Invalid transfer parameters.");
        }

        _logger.Information("Transferring {Amount} from {FromAccount} to {ToAccount}", amount, fromAccountNumber, toAccountNumber);
        return await _accountRepository.TransferAsync(fromAccountNumber, toAccountNumber, amount);
    }

    public async Task<bool> UndoTransferAsync(string fromAccountNumber, string toAccountNumber, double amount)
    {
        if (string.IsNullOrWhiteSpace(fromAccountNumber) || string.IsNullOrWhiteSpace(toAccountNumber) || amount <= 0)
        {
            _logger.Warning("Invalid undo transfer attempt: from {From}, to {To}, amount {Amount}", fromAccountNumber, toAccountNumber, amount);
            throw new ArgumentException("Invalid undo transfer parameters.");
        }

        _logger.Information("Undoing transfer of {Amount} from {FromAccount} to {ToAccount}", amount, fromAccountNumber, toAccountNumber);
        return await _accountRepository.UndoTransferAsync(fromAccountNumber, toAccountNumber, amount);
    }

    public async Task<List<Transaction>> GetAllTransactionsAsync()
    {
        _logger.Information("Fetching all transactions.");
        return await _accountRepository.GetAllTransactionsAsync();
    }
}
