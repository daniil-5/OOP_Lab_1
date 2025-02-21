namespace OOP_Lab_1.Core.Interfaces;

public interface IOperatorActions
{
    List<string> ViewTransactions();
    void CancelTransaction(string transactionId);
}