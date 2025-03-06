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
    
    public async Task<bool> AddLoanAsync(Loan loan, string bic)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = $@"
            INSERT INTO Loans 
            (LoanId, TypeOfLoan, TypeOfPercent, Percent, Amount, DurationMonths, Purpose, Approved, UserEmail, BIC, Timestamp) 
            VALUES 
            (@LoanId, @TypeOfLoan, @TypeOfPercent, @Percent, @Amount, @DurationMonths, @Purpose, @Approved, @UserEmail, @BIC ,CURRENT_TIMESTAMP)";

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
                command.Parameters.AddWithValue("@BIC", bic);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }

    public async Task<bool> CancelLoanAsync(string loanId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = $"DELETE FROM Loans WHERE LoanId = @LoanId";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@LoanId", loanId);
                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }

    public async Task<List<Loan>> GetLoansByEmailAsync(string userEmail, string bic)
    {
        var loans = new List<Loan>();

        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            var sql = $"SELECT * FROM Loans WHERE UserEmail = @UserEmail and BIC = @Bic";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@UserEmail", userEmail);
                command.Parameters.AddWithValue("@Bic", bic);

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
    
// Get all pending loans (not approved yet) for a specific BIC
public async Task<List<Loan>> GetPendingLoansAsync(string bic)
{
    var loans = new List<Loan>();

    try
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();
            
            var sql = @"SELECT * FROM Loans WHERE Approved = 0 AND BIC = @BIC";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@BIC", bic);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var loan = new Loan
                        {
                            LoanId = reader.GetString(reader.GetOrdinal("LoanId")),
                            TypeOfLoan = reader.GetInt32(reader.GetOrdinal("TypeOfLoan")),
                            TypeOfPercent = reader.GetInt32(reader.GetOrdinal("TypeOfPercent")),
                            Percent = reader.GetDouble(reader.GetOrdinal("Percent")),
                            Amount = reader.GetDouble(reader.GetOrdinal("Amount")),
                            DurationMonths = reader.GetInt32(reader.GetOrdinal("DurationMonths")),
                            Purpose = reader.GetString(reader.GetOrdinal("Purpose")),
                            UserEmail = reader.GetString(reader.GetOrdinal("UserEmail")),
                            Approved = false 
                        };

                        loans.Add(loan);
                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
        Console.WriteLine($"An error occurred while retrieving pending loans: {ex.Message}");
        throw; 
    }

    return loans;
}
    
    // Update the approval status of a loan (Approve or Disapprove)
    public async Task<bool> UpdateLoanApprovalStatusAsync(string loanId, bool isApproved)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();
            
            var sql = @"UPDATE Loans SET Approved = @Approved WHERE LoanId = @LoanId";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Approved", isApproved ? 1 : 0);
                command.Parameters.AddWithValue("@LoanId", loanId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }
    
    // Update the time stamp status of a loan
    public async Task<bool> UpdateLoanTimestampAsync(string loanId)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();
        
            var sql = @"UPDATE Loans SET Timestamp = CURRENT_TIMESTAMP WHERE LoanId = @LoanId";

            using (var command = new SqliteCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@LoanId", loanId);

                return await command.ExecuteNonQueryAsync() > 0;
            }
        }
    }
    
}