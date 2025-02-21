namespace OOP_Lab_1.Core.Interfaces;

public interface IManagerActions : IOperatorActions
{
    void ApproveLoan(string loanId);
    void CancelExternalTransaction(string transactionId);
}