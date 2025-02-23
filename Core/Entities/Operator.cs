using OOP_Lab_1.Core.Interfaces;

namespace OOP_Lab_1.Core.Entities;

public class Operator : User, IOperatorActions
{
    public Operator(User user) : base(user)
    {
        this.Role = 1; //Operator
    }

    public void ViewTransactions()
    {
        // List<string>
        Console.WriteLine($"{FullName} (Operator) is viewing transactions.");
    }

    public void ConfirmSalaryProject()
    {
        Console.WriteLine($"{FullName} (Operator) confirmed salary project.");
    }
}