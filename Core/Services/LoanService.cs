using Microsoft.Data.Sqlite;
using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;
using Serilog;

namespace OOP_Lab_1.Core.Services;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _loanRepository;
    private readonly ILogger _logger;

    // Constructor: accepts the ILoanRepository to handle data access
    public LoanService(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
        _logger = Log.ForContext<LoanService>();
    }

    // Fetch pending loans (not approved yet)
    public async Task<List<Loan>> GetPendingLoansAsync(string bic)
    {
        _logger.Information("Fetching pending loans for BIC: {BIC}", bic);
        var result = await _loanRepository.GetPendingLoansAsync(bic);
        _logger.Information("Fetched {Count} pending loans for BIC: {BIC}", result.Count, bic);
        return result;
    }

    // Update loan approval status (approve or disapprove)
    public async Task<bool> UpdateLoanApprovalStatusAsync(string loanId, bool isApproved)
    {
        _logger.Information("Updating loan approval status for LoanID: {LoanID}, Approved: {IsApproved}", loanId, isApproved);
        var result = await _loanRepository.UpdateLoanApprovalStatusAsync(loanId, isApproved);
        _logger.Information("Loan approval status updated: {Result}", result);
        return result;
    }

    // Cancel a loan (remove from database)
    public async Task<bool> CancelLoanAsync(string loanId)
    {
        _logger.Warning("Cancelling loan with LoanID: {LoanID}", loanId);
        var result = await _loanRepository.CancelLoanAsync(loanId);
        Log.Warning("Loan cancellation result: {Result}", result);
        return result;
    }

    // Add a new loan to the database
    public async Task<bool> AddLoanAsync(Loan loan, string bic)
    {
        _logger.Information("Adding new loan for BIC: {BIC}, Loan Amount: {Amount}", bic, loan.Amount);
        var result = await _loanRepository.AddLoanAsync(loan, bic);
        _logger.Information("Loan added: {Result}", result);
        return result;
    }

    // Get loans by user email (for a specific user)
    public async Task<List<Loan>> GetLoansByEmailAsync(string userEmail, string bic)
    {
        _logger.Information("Fetching loans for user: {UserEmail} and BIC: {BIC}", userEmail, bic);
        var result = await _loanRepository.GetLoansByEmailAsync(userEmail, bic);
        _logger.Information("Fetched {Count} loans for user: {UserEmail}", result.Count, userEmail);
        return result;
    }
    
    // Update the loan timestamp
    public async Task<bool> UpdateLoanTimestampAsync(string loanId)
    {
        _logger.Information("Updating timestamp for LoanID: {LoanID}", loanId);
        var result = await _loanRepository.UpdateLoanTimestampAsync(loanId);
        _logger.Information("Loan timestamp updated: {Result}", result);
        return result;
    }
}
