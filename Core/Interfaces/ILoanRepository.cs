using OOP_Lab_1.Core.Entities;

namespace OOP_Lab_1.Core.Interfaces;

public interface ILoanRepository
{
    // add the loan to the table
    Task<bool> AddLoanAsync(string tableName, Loan loan);
    
    // delete the loan from the table
    Task<bool> CancelLoanAsync(string tableName, string loanId);
    
    // return all not approved loans
    Task<List<Loan>> GetPendingLoansAsync(string tableName);
    
    // return all loans of the user by his email
    Task<List<Loan>> GetLoansByEmailAsync(string tableName, string userEmail);
    
    // updates the Approved status of the loan
    Task<bool> UpdateLoanApprovalStatusAsync(string tableName, string loanId, bool isApproved);
}