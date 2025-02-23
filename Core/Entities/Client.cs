using OOP_Lab_1.Core.Interfaces;
using SQLite;
namespace OOP_Lab_1.Core.Entities;

public class Client : User, IClientActions
{
    public Client(User user) : base(user)
    {
        this.Role = 0; // Client
    }
    public void Register()
    {
        Console.WriteLine($"{FullName} (Client) registered.");
    }

    public void OpenAccount()
    {
        Console.WriteLine($"{FullName} (Client) opened a new account.");
    }

    public void CloseAccount()
    {
        Console.WriteLine($"{FullName} (Client) closed an account.");
    }

    public void ApplyForLoan()
    {
        Console.WriteLine($"{FullName} (Client) applied for a loan.");
    }
    
    public void ApplyForSalaryProject()
    {
        Console.WriteLine($"{FullName} (Client) applied for salary project.");
    }
}