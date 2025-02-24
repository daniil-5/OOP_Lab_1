using OOP_Lab_1.Core.Interfaces;
using SQLite;
namespace OOP_Lab_1.Core.Entities;

public class Client : User, IClientActions
{
    public Client(User user) : base(user)
    {
        this.Role = 0; // Clien
    }

    void OpenAccountAsync(UserAccount account, string tableName)
    {
        Console.WriteLine($"{FullName} (Client) opened an account.");
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