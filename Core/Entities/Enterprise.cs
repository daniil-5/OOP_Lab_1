using SQLite;
namespace OOP_Lab_1.Core.Entities;

public class Enterprise
{
    public string Type { get; set; }
    public string LegalName { get; set; }
    public string UNP { get; set; }
    public string BIC { get; set; }
    public string LegalAddress { get; set; }
    public Bank Bank { get; set; }
    public List<Account> Accounts { get; private set; } = new List<Account>();
    public List<Client> Employees { get; private set; } = new List<Client>();

    public void ApplyForSalaryProject(){}
}