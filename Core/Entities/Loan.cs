using OOP_Lab_1.Core.Interfaces;
using SQLite;
namespace OOP_Lab_1.Core.Entities;

public class Loan : ILoan
{
    public string LoanId { get; set; }
    public int TypeOfLoan { get; set; }
    public int TypeOfPercent { get; set; }
    public double Percent { get; set; }
    public double Amount { get; set; }
    public int DurationMonths { get; set; }
    public string Purpose { get; set; }
    public bool Approved { get; set; }
    
    public string UserEmail { get; set; }

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