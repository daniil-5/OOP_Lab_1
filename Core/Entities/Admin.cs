using OOP_Lab_1.Core.Interfaces;

namespace OOP_Lab_1.Core.Entities;

public class Admin : User, IAdminActions
{
    public Admin(User user) : base(user)
    {
        this.Role = 4; // Admin
    }
    public List<string> ViewLogs()
    {
        Console.WriteLine($"{FullName} (Admin) is viewing logs.");
        return new List<string>();
    }

    public void CancelAllTransactions()
    {
        Console.WriteLine($"{FullName} (Admin) canceled all transactions.");
    }
    public void CancelUserAction(string actionId)
    {
        Console.WriteLine($"{FullName} (Admin) canceled user action with id {actionId}.");
    }
}