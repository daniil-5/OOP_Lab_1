using OOP_Lab_1.Core.Interfaces;

namespace OOP_Lab_1.Core.Entities;

public class Manager : User, IOperatorActions, IManagerActions
{
    public Manager(User user) : base(user)
    {
        Role = 2; // Manager
    }

    public void ApproveLoan()
    {
        //string loan_id
        Console.WriteLine($"{FullName} (Manager) approved loan.");
    }

    public void CancelExternalTransaction()
    {
        // string transactionId
        Console.WriteLine($"{FullName} (Manager) canceled external transaction .");
    }
    public void ViewTransactions()
    {
        // List<string>
        Console.WriteLine($"{FullName} (Manager) is viewing transactions.");
    }

    public void ConfirmSalaryProject()
    {
        Console.WriteLine($"{FullName} (Manager) confirmed salary project.");
    }
}