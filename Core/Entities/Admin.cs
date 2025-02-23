using OOP_Lab_1.Core.Interfaces;

namespace OOP_Lab_1.Core.Entities;

public class Admin : User, IAdminActions
{
    public Admin(User user) : base(user)
    {
        this.Role = 4; // Admin
    }
    public void ViewLogs()
    {
        Console.WriteLine($"{FullName} (Admin) is viewing logs.");
    }
    
    public void CancelUserActions()
    {
        // string actionId
        Console.WriteLine($"{FullName} (Admin) canceled user action with id.");
    }
}