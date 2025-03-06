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
    public async Task<List<Loan>> GetPendingLoansAsync(string bic)
    {
        return await _loanRepository.GetPendingLoansAsync(bic);
    }

    // Update loan approval status (approve or disapprove)
    public async Task<bool> UpdateLoanApprovalStatusAsync(string loanId, bool isApproved)
    {
        return await _loanRepository.UpdateLoanApprovalStatusAsync(loanId, isApproved);
    }

    // Cancel a loan (remove from database)
    public async Task<bool> CancelLoanAsync(string loanId)
    {
        return await _loanRepository.CancelLoanAsync(loanId);
    }

    // Add a new loan to the database
    public async Task<bool> AddLoanAsync(Loan loan, string bic)
    {
        return await _loanRepository.AddLoanAsync(loan, bic);
    }

    // Get loans by user email (for a specific user)
    public async Task<List<Loan>> GetLoansByEmailAsync(string userEmail, string bic)
    {
        return await _loanRepository.GetLoansByEmailAsync(userEmail, bic);
    }
    
    // Update the loan timestamp
    public async Task<bool> UpdateLoanTimestampAsync(string loanId)
    {
        return await _loanRepository.UpdateLoanTimestampAsync(loanId);
    }
}