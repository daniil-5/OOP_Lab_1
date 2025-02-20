using SQLite;
namespace OOP_Lab_1.Core.Entities;

public class Loan
{
    public decimal Amount { get; set; }
    public double InterestRate { get; set; }
    public int TermMonths { get; set; }
    public bool Approved { get; private set; }
    public Client Borrower { get; set; }
    public Bank IssuingBank { get; set; }

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