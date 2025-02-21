using OOP_Lab_1.Core.Interfaces;

namespace OOP_Lab_1.Core.Entities;

public class Operator : User, IOperatorActions
{
    public Operator(User user) : base(user)
    {
        this.Role = 1; //Operator
    }

    public List<string> ViewTransactions()
    {
        Console.WriteLine($"{FullName} (Operator) is viewing transactions.");
        return new List<string>();
    }

    public void CancelTransaction(string transactionId)
    {
        Console.WriteLine($"{FullName} (Operator) canceled transaction {transactionId}.");
    }
}