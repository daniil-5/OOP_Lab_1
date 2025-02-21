using OOP_Lab_1.Core.Interfaces;

namespace OOP_Lab_1.Core.Entities;

public class Manager : Operator, IManagerActions
{
    public Manager(User user) : base(user)
    {
        Role = 2; // Manager
    }

    public void ApproveLoan(string loanId)
    {
        Console.WriteLine($"{FullName} (Manager) approved loan {loanId}.");
    }

    public void CancelExternalTransaction(string transactionId)
    {
        Console.WriteLine($"{FullName} (Manager) canceled external transaction {transactionId}.");
    }
}