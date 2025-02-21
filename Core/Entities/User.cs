using SQLite;
namespace OOP_Lab_1.Core.Entities;

public class User
{
    public User() { }
    protected User(User user)
    {
        Id = user.Id;
        FullName = user.FullName;
        PassportNumber = user.PassportNumber;
        IdentificationNumber = user.IdentificationNumber;
        Phone = user.Phone;
        Email = user.Email;
        Password = user.Password;
        Role = user.Role;
        Accounts = user.Accounts;
    }
    public int Id { get; set; }
    public string FullName { get; set; }
    public string PassportNumber { get; set; }
    public string IdentificationNumber { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int Role { get; set; }
    
    public List<UserAccount> Accounts;
}
