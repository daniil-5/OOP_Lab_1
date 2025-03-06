using OOP_Lab_1.Core.Entities;

namespace OOP_Lab_1.Core.Interfaces;

public interface ILoanService
{ 
    // add the loan to the table
    Task<bool> AddLoanAsync(Loan loan, string bic);
    
    // delete the loan from the table
    Task<bool> CancelLoanAsync(string loanId);
    
    // return all not approved loans
    Task<List<Loan>> GetPendingLoansAsync(string bic);
    
    // return all loans of the user by his email
    Task<List<Loan>> GetLoansByEmailAsync(string userEmail, string bic);
    
    // updates the Approved status of the loan
    Task<bool> UpdateLoanApprovalStatusAsync(string loanId, bool isApproved);
    
    // updates the time stamp of the loan
    Task<bool> UpdateLoanTimestampAsync(string loanId);
}