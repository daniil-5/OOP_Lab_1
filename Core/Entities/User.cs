using SQLite;
namespace OOP_Lab_1.Core.Entities;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string PassportNumber { get; set; }
    public string IdentificationNumber { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int Role { get; set; }
    
    
}
