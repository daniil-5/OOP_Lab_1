using Microsoft.Data.Sqlite;
using OOP_Lab_1.Core.Interfaces;

namespace OOP_Lab_1.Core.Entities.Repositories;

public class LoanRepository : ILoanRepository
{
    private readonly string _connectionString;
    
    public LoanRepository(string databasePath)
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
    
    public async Task<bool> AddLoanAsync(string tableName, Loan loan)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = $@"
            INSERT INTO {tableName} 
            (LoanId, TypeOfLoan, TypeOfPercent, Percent, Amount, DurationMonths, Purpose, Approved, UserEmail, Timestamp) 
            VALUES 
            (@LoanId, @TypeOfLoan, @TypeOfPercent, @Percent, @Amount, @DurationMonths, @Purpose, @Approved, @UserEmail, CURRENT_TIMESTAMP)";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@LoanId", loan.LoanId);
                command.Parameters.AddWithValue("@TypeOfLoan", loan.TypeOfLoan);
                command.Parameters.AddWithValue("@TypeOfPercent", loan.TypeOfPercent);
                command.Parameters.AddWithValue("@Percent", loan.Percent);
                command.Parameters.AddWithValue("@Amount", loan.Amount);
                command.Parameters.AddWithValue("@DurationMonths", loan.DurationMonths);
                command.Parameters.AddWithValue("@Purpose", loan.Purpose);
                command.Parameters.AddWithValue("@Approved", loan.Approved);
                command.Parameters.AddWithValue("@UserEmail", loan.UserEmail);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }

    public async Task<bool> CancelLoanAsync(string tableName, string loanId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = $"DELETE FROM {tableName} WHERE LoanId = @LoanId";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@LoanId", loanId);
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }

    public async Task<List<Loan>> GetLoansByEmailAsync(string tableName, string userEmail)
    {
        var loans = new List<Loan>();

        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = $"SELECT * FROM {tableName} WHERE UserEmail = @UserEmail";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@UserEmail", userEmail);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        loans.Add(new Loan
                        {
                            LoanId = reader.GetString(1),
                            TypeOfLoan = reader.GetInt32(2),
                            TypeOfPercent = reader.GetInt32(3),
                            Percent = reader.GetDouble(4),
                            Amount = reader.GetDouble(5),
                            DurationMonths = reader.GetInt32(6),
                            Purpose = reader.GetString(7),
                            Approved = reader.GetBoolean(8),
                            UserEmail = reader.GetString(9)
                        });
                    }
                }
            }
        }

        return loans;
    }
    
    // Get all pending loans (not approved yet)
    public async Task<List<Loan>> GetPendingLoansAsync(string tableName)
    {
        var loans = new List<Loan>();

        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();
            
            var sql = $"SELECT * FROM {tableName} WHERE Approved = 0";  // Retrieve only unapproved loans
            
            using (var command = new SqliteCommand(sql, connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var loan = new Loan
                    {
                        LoanId = reader.GetString(1),
                        TypeOfLoan = reader.GetInt32(2),
                        TypeOfPercent = reader.GetInt32(3),
                        Percent = reader.GetDouble(4),
                        Amount = reader.GetDouble(5),
                        DurationMonths = reader.GetInt32(6),
                        Purpose = reader.GetString(7),
                        UserEmail = reader.GetString(8),
                        Approved = false
                    };
                    loans.Add(loan);
                }
            }
        }

        return loans;
    }
    
    // Update the approval status of a loan (Approve or Disapprove)
    public async Task<bool> UpdateLoanApprovalStatusAsync(string tableName, string loanId, bool isApproved)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();
            
            var sql = $"UPDATE {tableName} SET Approved = @Approved WHERE LoanId = @LoanId";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Approved", isApproved ? 1 : 0);
                command.Parameters.AddWithValue("@LoanId", loanId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }
}