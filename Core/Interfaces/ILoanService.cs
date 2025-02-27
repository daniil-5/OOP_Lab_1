using OOP_Lab_1.Core.Entities;

namespace OOP_Lab_1.Core.Interfaces;

public interface ILoanService
{ 
    // Fetch pending loans (not approved yet)
    Task<List<Loan>> GetPendingLoansAsync(string tableName);

    // Update loan approval status (approve or disapprove)
    Task<bool> UpdateLoanApprovalStatusAsync(string tableName, string loanId, bool isApproved);

    // Cancel a loan (remove from database)
    Task<bool> CancelLoanAsync(string tableName, string loanId);

    // Add a new loan to the database
    Task<bool> AddLoanAsync(string tableName, Loan loan);
    
    // Get loans by user email (for a specific user)
    Task<List<Loan>> GetLoansByUserEmailAsync(string tableName, string userEmail);

}