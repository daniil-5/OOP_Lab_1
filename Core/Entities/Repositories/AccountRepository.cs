using Microsoft.Data.Sqlite;
using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;

namespace OOP_Lab_1.Core.Services;

public class AccountRepository :IAccountRepository
{
    private readonly string _connectionString;
    
   public AccountRepository(string databasePath)
    {
        _connectionString = $"Data Source={databasePath}";
        InitializeAccountsDatabase();
    }
   
   private void InitializeAccountsDatabase()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
        }
    }
    
   public async Task<bool> AddAccountAsync(UserAccount account)
{
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
            var checkSql = "SELECT COUNT(*) FROM UserAccounts WHERE AccountNumber = @AccountNumber";
            using (var checkCommand = new SqliteCommand(checkSql, connection))
            {
                checkCommand.Parameters.AddWithValue("@AccountNumber", account.AccountNumber);
                var count = (long)(await checkCommand.ExecuteScalarAsync())!;
                if (count > 0)
                    throw new InvalidOperationException("AccountNumber already exists.");
            }
            
            var sql = @"
                INSERT INTO UserAccounts (AccountNumber, Balance, IsBlocked, IsFrozen, UserEmail, BIC)
                VALUES (@AccountNumber, @Balance, @IsBlocked, @IsFrozen, @UserEmail, @BIC)";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@AccountNumber", account.AccountNumber);
                command.Parameters.AddWithValue("@Balance", account.Balance);
                command.Parameters.AddWithValue("@IsBlocked", account.IsBlocked);
                command.Parameters.AddWithValue("@IsFrozen", account.IsFrozen);
                command.Parameters.AddWithValue("@UserEmail", account.UserEmail);
                command.Parameters.AddWithValue("@BIC", account.BIC);

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
    
   public async Task<UserAccount> GetAccountAsync(string accountNumber)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = $@"
                SELECT * FROM UserAccounts WHERE AccountNumber = @AccountNumber";

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
                            reader.GetString(5),
                            reader.GetString(6));
                    }
                }
            }
        }

        return null; // UserAccount not found
    }
   
   public async Task<bool> DeleteAccountAsync(string accountNumber)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
            throw new ArgumentException("AccountNumber cannot be null or empty.", nameof(accountNumber));

        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                
                var sql = @"
                DELETE FROM UserAccounts 
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
   public async Task<List<UserAccount>> GetAccountsByEmailAsync(string email, string bankId)
    {
        List<UserAccount> accounts = new List<UserAccount>();

        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = @"
            SELECT * FROM UserAccounts WHERE (UserEmail = @UserEmail and BIC = @BIC)";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@UserEmail", email);
                command.Parameters.AddWithValue("@BIC", bankId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        accounts.Add(new UserAccount (
                            reader.GetString(1),
                            reader.GetDouble(2),
                            reader.GetBoolean(3),
                            reader.GetBoolean(4),
                            reader.GetString(5),
                            reader.GetString(6)));
                    }
                }
            }
        }

        return accounts;  // Returns the list of accounts with the given email
    }
   
    public async Task<List<UserAccount>> GetAllAccountsAsync()
    {
        List<UserAccount> accounts = new List<UserAccount>();

        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = @"SELECT AccountNumber, Balance, IsBlocked, IsFrozen, UserEmail, BIC 
                        FROM UserAccounts"; // Explicitly list columns for clarity

            using (var command = new SqliteCommand(sql, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        accounts.Add(new UserAccount (
                            reader.GetString(0),
                            reader.GetDouble(1),
                            reader.GetBoolean(2),
                            reader.GetBoolean(3),
                            reader.GetString(4),
                            reader.GetString(5)));
                    }
                }
            }
        }

        return accounts;  // Returns the list of all accounts
    }
    
   public async Task<bool> WithdrawAsync(string accountId, double amount)
{
    // Validate input parameters
    if (string.IsNullOrEmpty(accountId))
        throw new ArgumentException("Account ID cannot be null or empty.", nameof(accountId));

    if (amount <= 0)
        throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

    using (var connection = new SqliteConnection(_connectionString))
    {
        await connection.OpenAsync();

        // Check account status and balance
        var checkSql = @"
            SELECT Balance, IsBlocked, IsFrozen 
            FROM UserAccounts 
            WHERE AccountNumber = @AccountNumber;";

        using (var checkCommand = new SqliteCommand(checkSql, connection))
        {
            checkCommand.Parameters.AddWithValue("@AccountNumber", accountId);

            using (var reader = await checkCommand.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    double balance = reader.GetDouble(0);
                    bool isBlocked = reader.GetBoolean(1);
                    bool isFrozen = reader.GetBoolean(2);

                    // Check if the account is blocked or frozen
                    if (isBlocked || isFrozen)
                    {
                        return false; // Withdrawal denied: Account is blocked or frozen
                    }

                    // Check if there are sufficient funds
                    if (balance < amount)
                    {
                        return false; // Insufficient funds
                    }

                    // Deduct the amount from the account balance
                    var updateSql = @"
                        UPDATE UserAccounts 
                        SET Balance = Balance - @Amount 
                        WHERE AccountNumber = @AccountNumber;";

                    using (var updateCommand = new SqliteCommand(updateSql, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@Amount", amount);
                        updateCommand.Parameters.AddWithValue("@AccountNumber", accountId);

                        int rowsAffected = await updateCommand.ExecuteNonQueryAsync();
                        return rowsAffected > 0; // Return true if withdrawal was successful
                    }
                }
                else
                {
                    // Account not found
                    return false;
                }
            }
        }
    }
}
public async Task<bool> TransferAsync(string fromAccountId, string toAccountId, double amount)
{
    // Validate input parameters
    if (string.IsNullOrEmpty(fromAccountId))
        throw new ArgumentException("Source account ID cannot be null or empty.", nameof(fromAccountId));

    if (string.IsNullOrEmpty(toAccountId))
        throw new ArgumentException("Destination account ID cannot be null or empty.", nameof(toAccountId));

    if (amount <= 0)
        throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

    // Check if source and destination accounts are the same
    if (fromAccountId == toAccountId)
    {
        return false; // Transfer denied: Source and destination accounts are the same
    }

    using (var connection = new SqliteConnection(_connectionString))
    {
        await connection.OpenAsync();

        // Begin a transaction
        using (var transaction = (SqliteTransaction)await connection.BeginTransactionAsync())
        {
            try
            {
                // Check source account status and balance
                var fromAccountSql = @"
                    SELECT Balance, IsBlocked, IsFrozen 
                    FROM UserAccounts 
                    WHERE AccountNumber = @FromAccountId;";

                using (var fromAccountCommand = new SqliteCommand(fromAccountSql, connection, transaction))
                {
                    fromAccountCommand.Parameters.AddWithValue("@FromAccountId", fromAccountId);

                    using (var fromAccountReader = await fromAccountCommand.ExecuteReaderAsync())
                    {
                        if (!await fromAccountReader.ReadAsync())
                        {
                            await transaction.RollbackAsync(); // Rollback the transaction
                            return false; // Source account not found
                        }

                        double fromBalance = fromAccountReader.GetDouble(0);
                        bool isFromBlocked = fromAccountReader.GetBoolean(1);
                        bool isFromFrozen = fromAccountReader.GetBoolean(2);

                        // Check if the source account is blocked or frozen
                        if (isFromBlocked || isFromFrozen)
                        {
                            await transaction.RollbackAsync(); // Rollback the transaction
                            return false; // Transfer denied: Source account is blocked or frozen
                        }

                        // Check if the source account has sufficient funds
                        if (fromBalance < amount)
                        {
                            await transaction.RollbackAsync(); // Rollback the transaction
                            return false; // Insufficient funds
                        }
                    }
                }

                // Check destination account status
                var toAccountSql = @"
                    SELECT IsBlocked, IsFrozen 
                    FROM UserAccounts 
                    WHERE AccountNumber = @ToAccountId;";

                using (var toAccountCommand = new SqliteCommand(toAccountSql, connection, transaction))
                {
                    toAccountCommand.Parameters.AddWithValue("@ToAccountId", toAccountId);

                    using (var toAccountReader = await toAccountCommand.ExecuteReaderAsync())
                    {
                        if (!await toAccountReader.ReadAsync())
                        {
                            await transaction.RollbackAsync(); // Rollback the transaction
                            return false; // Destination account not found
                        }

                        bool isToBlocked = toAccountReader.GetBoolean(0);
                        bool isToFrozen = toAccountReader.GetBoolean(1);

                        // Check if the destination account is blocked or frozen
                        if (isToBlocked || isToFrozen)
                        {
                            await transaction.RollbackAsync(); // Rollback the transaction
                            return false; // Transfer denied: Destination account is blocked or frozen
                        }
                    }
                }

                // Deduct the amount from the source account
                var deductSql = @"
                    UPDATE UserAccounts 
                    SET Balance = Balance - @Amount 
                    WHERE AccountNumber = @FromAccountId;";

                using (var deductCommand = new SqliteCommand(deductSql, connection, transaction))
                {
                    deductCommand.Parameters.AddWithValue("@Amount", amount);
                    deductCommand.Parameters.AddWithValue("@FromAccountId", fromAccountId);

                    int deductRowsAffected = await deductCommand.ExecuteNonQueryAsync();
                    if (deductRowsAffected == 0)
                    {
                        await transaction.RollbackAsync(); // Rollback the transaction
                        return false; // Failed to deduct from source account
                    }
                }

                // Add the amount to the destination account
                var addSql = @"
                    UPDATE UserAccounts 
                    SET Balance = Balance + @Amount 
                    WHERE AccountNumber = @ToAccountId;";

                using (var addCommand = new SqliteCommand(addSql, connection, transaction))
                {
                    addCommand.Parameters.AddWithValue("@Amount", amount);
                    addCommand.Parameters.AddWithValue("@ToAccountId", toAccountId);

                    int addRowsAffected = await addCommand.ExecuteNonQueryAsync();
                    if (addRowsAffected == 0)
                    {
                        await transaction.RollbackAsync(); // Rollback the transaction
                        return false; // Failed to add to destination account
                    }
                }

                // Log the transfer (optional)
                var logTransferSql = @"
                    INSERT INTO Transfers (FromAccountId, ToAccountId, Amount, Status) 
                    VALUES (@FromAccountId, @ToAccountId, @Amount, @Status);";

                using (var logTransferCommand = new SqliteCommand(logTransferSql, connection, transaction))
                {
                    logTransferCommand.Parameters.AddWithValue("@FromAccountId", fromAccountId);
                    logTransferCommand.Parameters.AddWithValue("@ToAccountId", toAccountId);
                    logTransferCommand.Parameters.AddWithValue("@Amount", amount);
                    logTransferCommand.Parameters.AddWithValue("@Status", "Success");

                    await logTransferCommand.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync(); // Commit the transaction
                return true; // Transfer successful
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(); // Rollback the transaction in case of an error
                throw; // Re-throw the exception
            }
        }
    }
}

public async Task<bool> RefillAsync(string accountId, double amount)
{
    // Validate input parameters
    if (string.IsNullOrEmpty(accountId))
        throw new ArgumentException("Account ID cannot be null or empty.", nameof(accountId));

    if (amount <= 0)
        throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

    using (var connection = new SqliteConnection(_connectionString))
    {
        await connection.OpenAsync();

        // Begin a transaction
        using (var transaction = (SqliteTransaction)await connection.BeginTransactionAsync())
        {
            try
            {
                // Check account status
                var checkSql = @"
                    SELECT IsBlocked, IsFrozen 
                    FROM UserAccounts 
                    WHERE AccountNumber = @AccountNumber;";

                using (var checkCommand = new SqliteCommand(checkSql, connection, transaction))
                {
                    checkCommand.Parameters.AddWithValue("@AccountNumber", accountId);

                    using (var reader = await checkCommand.ExecuteReaderAsync())
                    {
                        if (!await reader.ReadAsync())
                        {
                            await transaction.RollbackAsync(); // Rollback the transaction
                            return false; // Account not found
                        }

                        bool isBlocked = reader.GetBoolean(0);
                        bool isFrozen = reader.GetBoolean(1);

                        // Check if the account is blocked or frozen
                        if (isBlocked || isFrozen)
                        {
                            await transaction.RollbackAsync(); // Rollback the transaction
                            return false; // Refill denied: Account is blocked or frozen
                        }
                    }
                }

                // Add the amount to the account balance
                var updateSql = @"
                    UPDATE UserAccounts 
                    SET Balance = Balance + @Amount 
                    WHERE AccountNumber = @AccountNumber;";

                using (var updateCommand = new SqliteCommand(updateSql, connection, transaction))
                {
                    updateCommand.Parameters.AddWithValue("@Amount", amount);
                    updateCommand.Parameters.AddWithValue("@AccountNumber", accountId);

                    int rowsAffected = await updateCommand.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        await transaction.RollbackAsync(); // Rollback the transaction
                        return false; // Failed to refill account
                    }
                }

                await transaction.CommitAsync(); // Commit the transaction
                return true; // Refill successful
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(); // Rollback the transaction in case of an error
                throw; // Re-throw the exception
            }
        }
    }
}
}