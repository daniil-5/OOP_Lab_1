using SQLite;
namespace OOP_Lab_1.Core.Entities;

public class Client : User
{
    public List<Account> Accounts { get; private set; } = new List<Account>();
    public List<Bank> Banks { get; private set; } = new List<Bank>();

    public void OpenAccount(Account account)
    {
        Accounts.Add(account);
    }

    public void CloseAccount(Account account)
    {
        Accounts.Remove(account);
    }

    public void ApplyForLoan(Loan loan)
    {
        Console.WriteLine($"{FullName} applied for a loan of {loan.Amount}.");
    }

    public void ApplyForSalaryProject()
    {
        Console.WriteLine($"{FullName} applied for a salary project.");
    }
}