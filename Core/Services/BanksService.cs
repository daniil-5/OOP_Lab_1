using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;


namespace OOP_Lab_1.Core.Services;

public class BanksService : IBanksService
{
    private readonly string _connectionString;

    public BanksService(string databasePath)
    {
        _connectionString = $"Data Source={databasePath}";
        InitializeBanksDatabase();
    }
    
    public async Task CreateBankTableAsync(string id)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();
            
            var sql = $@"
            CREATE TABLE IF NOT EXISTS {id} (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                FullName TEXT NOT NULL,
                PassportNumber TEXT NOT NULL,
                IdentificationNumber TEXT NOT NULL,
                Phone TEXT NOT NULL,
                Email TEXT NOT NULL UNIQUE,
                Password TEXT NOT NULL,
                Role INTEGER NOT NULL
            );";

            using (var command = new SqliteCommand(sql, connection))
            {
                await command.ExecuteNonQueryAsync();
            }
        }
    }
    
    private void InitializeBanksDatabase()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
        }
    }

    public async Task<List<(string BankId, string BankName)>> GetBanksAsync()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = "SELECT BIC, Name FROM Banks"; 

            using (var command = new SqliteCommand(sql, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var banks = new List<(string, string)>();

                    while (await reader.ReadAsync())
                    {
                        string bankId = reader.GetString(0);
                        string bankName = reader.GetString(1);
                        banks.Add((bankId, bankName));
                    }

                    return banks;
                }
            }
        }
    }

    public async Task<bool> AddBankAsync(Bank bank)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = "INSERT INTO Banks (Name, BIC) " +
                      "VALUES (@Name, @BIC)";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Name", bank.Name);
                command.Parameters.AddWithValue("@BIC", bank.BIC);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }
    public async Task<bool> RemoveBankAsync(string bankName)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = "DELETE FROM Banks WHERE Name = @Name";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Name", bankName);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }
    public async Task<bool> UpdateBankAsync(Bank bank)
    {
        if (bank == null || string.IsNullOrEmpty(bank.Name))
        {
            throw new ArgumentNullException(nameof(bank), "Bank or name cannot be null.");
        }

        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sql = "UPDATE Banks SET BIC = @BIC WHERE Name = @Name";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@BIC", bank.BIC);
                    command.Parameters.AddWithValue("@Name", bank.Name);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating bank: {ex.Message}");
            return false;
        }
    }
    public async Task CreateAccountTableAsync()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = @"
                CREATE TABLE IF NOT EXISTS UserAccounts (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    AccountNumber TEXT NOT NULL UNIQUE,
                    Balance REAL NOT NULL,
                    IsBlocked BOOLEAN DEFAULT 0,
                    IsFrozen BOOLEAN DEFAULT 0,
                    UserEmail TEXT NOT NULL,
                    BIC TEXT NOT NULL                                                         
                );";

            using (var command = new SqliteCommand(sql, connection))
            {
                await command.ExecuteNonQueryAsync();
            }
        }
    }
    public async Task CreateLoanTableAsync()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = @"
                CREATE TABLE IF NOT EXISTS Loans (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    LoanId TEXT NOT NULL UNIQUE,
                    TypeOfLoan INTEGER NOT NULL,
                    TypeOfPercent INTEGER NOT NULL,
                    Percent REAL NOT NULL,
                    Amount REAL NOT NULL,
                    DurationMonths INTEGER NOT NULL,
                    Purpose TEXT NOT NULL,
                    Approved BOOLEAN DEFAULT 0,
                    UserEmail TEXT NOT NULL,
                    BIC TEXT NOT NULL,
                    Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP
                );";

            using (var command = new SqliteCommand(sql, connection))
            {
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}