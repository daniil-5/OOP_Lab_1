using Microsoft.Data.Sqlite;
using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;

namespace OOP_Lab_1.Core.Services;

public class LoanService :ILoanService
{
    private readonly ILoanRepository _loanRepository;

    // Constructor: accepts the ILoanRepository to handle data access
    public LoanService(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }

    // Fetch pending loans (not approved yet)
    public async Task<List<Loan>> GetPendingLoansAsync(string tableName)
    {
        return await _loanRepository.GetPendingLoansAsync(tableName);
    }

    // Update loan approval status (approve or disapprove)
    public async Task<bool> UpdateLoanApprovalStatusAsync(string tableName, string loanId, bool isApproved)
    {
        return await _loanRepository.UpdateLoanApprovalStatusAsync(tableName, loanId, isApproved);
    }

    // Cancel a loan (remove from database)
    public async Task<bool> CancelLoanAsync(string tableName, string loanId)
    {
        return await _loanRepository.CancelLoanAsync(tableName, loanId);
    }

    // Add a new loan to the database
    public async Task<bool> AddLoanAsync(string tableName, Loan loan)
    {
        return await _loanRepository.AddLoanAsync(tableName, loan);
    }

    // Get loans by user email (for a specific user)
    public async Task<List<Loan>> GetLoansByUserEmailAsync(string tableName, string userEmail)
    {
        return await _loanRepository.GetLoansByEmailAsync(tableName, userEmail);
    }
}