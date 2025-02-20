using OOP_Lab_1.Core.Interfaces;
using SQLite;
namespace OOP_Lab_1.Core.Entities;

public class Loan : ILoan
{
    public double Amount { get; set; }
    public int DurationMonths { get; set; }
    public double InterestRate { get; set; }
    public bool Approved { get; private set; }
    public IUser Borrower { get; set; }
    public IBank IssuingBank { get; set; }

    public void Approve()
    {
        Approved = true;
        Console.WriteLine($"Loan of {Amount} approved.");
    }

    public void Reject()
    {
        Approved = false;
        Console.WriteLine($"Loan of {Amount} rejected.");
    }
}