namespace OOP_Lab_1.Core.Interfaces;

public interface IUser
{
     int Id { get; set; }
     string FullName { get; set; }
     string PassportNumber { get; set; }
     string IdentificationNumber { get; set; }
     string Phone { get; set; }
     string Email { get; set; }
     string Password { get; set; }
     int Role { get; set; }
}