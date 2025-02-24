using Microsoft.Data.Sqlite;
using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;

namespace OOP_Lab_1.Core.Services;

public class AccountService : IAccountService
{
    private readonly string _connectionString;
    
    public AccountService(string databasePath)
    {
        _connectionString = $"Data Source={databasePath}";
        InitializeAccountsDatabase();
    }

    // Initialize the database (can be used for setup tasks)
    private void InitializeAccountsDatabase()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
        }
    }

    // Method to insert a new account into a specific bank account table
    public async Task<bool> AddAccountAsync(string tableName, UserAccount account)
{
    if (string.IsNullOrWhiteSpace(tableName))
        throw new ArgumentException("Table name cannot be null or empty.", nameof(tableName));
    if (account == null)
        throw new ArgumentNullException(nameof(account));
    if (string.IsNullOrEmpty(account.AccountNumber))
        throw new ArgumentException("AccountNumber cannot be null or empty.");
    if (string.IsNullOrEmpty(account.UserEmail))
        throw new ArgumentException("UserEmail cannot be null or empty.");

    try
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            // Check for duplicate AccountNumber
            var checkSql = $"SELECT COUNT(*) FROM {tableName} WHERE AccountNumber = @AccountNumber";
            using (var checkCommand = new SqliteCommand(checkSql, connection))
            {
                checkCommand.Parameters.AddWithValue("@AccountNumber", account.AccountNumber);
                var count = (long)await checkCommand.ExecuteScalarAsync();
                if (count > 0)
                    throw new InvalidOperationException("AccountNumber already exists.");
            }
            
            var sql = $@"
                INSERT INTO {tableName} (AccountNumber, Balance, IsBlocked, IsFrozen, UserEmail)
                VALUES (@AccountNumber, @Balance, @IsBlocked, @IsFrozen, @UserEmail)";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@AccountNumber", account.AccountNumber);
                command.Parameters.AddWithValue("@Balance", account.Balance);
                command.Parameters.AddWithValue("@IsBlocked", account.IsBlocked);
                command.Parameters.AddWithValue("@IsFrozen", account.IsFrozen);
                command.Parameters.AddWithValue("@UserEmail", account.UserEmail);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }
    catch (Exception ex)
    {
        // Log the exception (use a proper logging framework in production)
        Console.WriteLine($"Error adding account: {ex.Message}");
        throw; // Re-throw the exception for the caller to handle
    }
}

    // Method to get account details by account number
    public async Task<Account> GetAccountAsync(string tableName, string accountNumber)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = $@"
                SELECT * FROM {tableName} WHERE AccountNumber = @AccountNumber";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@AccountNumber", accountNumber);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new UserAccount (
                            reader.GetString(1),
                            reader.GetDouble(2),
                            reader.GetBoolean(3),
                            reader.GetBoolean(4),
                            reader.GetString(5));
                    }
                }
            }
        }

        return null; // UserAccount not found
    }

    // Method to update account balance and status (e.g., IsBlocked, IsFrozen)
    public async Task<bool> UpdateAccountAsync(string tableName, string accountNumber, double balance, bool isBlocked, bool isFrozen, string email)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = $@"
                UPDATE {tableName}
                SET Balance = @Balance, IsBlocked = @IsBlocked, IsFrozen = @IsFrozen, UserEmail = @UserEmail
                WHERE AccountNumber = @AccountNumber";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Balance", balance);
                command.Parameters.AddWithValue("@IsBlocked", isBlocked);
                command.Parameters.AddWithValue("@IsFrozen", isFrozen);
                command.Parameters.AddWithValue("@UserEmail", email);
                command.Parameters.AddWithValue("@AccountNumber", accountNumber);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }

    // Method to delete an account by account number
    public async Task<bool> DeleteAccountAsync(string tableName, string accountNumber)
    {
        if (string.IsNullOrWhiteSpace(tableName))
            throw new ArgumentException("Table name cannot be null or empty.", nameof(tableName));
        if (string.IsNullOrWhiteSpace(accountNumber))
            throw new ArgumentException("AccountNumber cannot be null or empty.", nameof(accountNumber));

        try
        {
            // Console.WriteLine(tableName);
            // Console.WriteLine(accountNumber);
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                var sql = $@"
                DELETE FROM {tableName} 
                WHERE AccountNumber = @AccountNumber";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting account: {ex.Message}");
            throw;
        }
    }
    
    public async Task<List<UserAccount>> GetAccountsByEmailAsync(string tableName, string email)
    {
        List<UserAccount> accounts = new List<UserAccount>();

        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = $@"
            SELECT * FROM {tableName} WHERE UserEmail = @UserEmail";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@UserEmail", email);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        accounts.Add(new UserAccount (
                            reader.GetString(1),
                            reader.GetDouble(2),
                            reader.GetBoolean(3),
                            reader.GetBoolean(4),
                            reader.GetString(5)));
                    }
                }
            }
        }

        return accounts;  // Returns the list of accounts with the given email
    }

}
