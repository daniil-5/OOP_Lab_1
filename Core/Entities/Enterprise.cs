using OOP_Lab_1.Core.Interfaces;
using SQLite;
namespace OOP_Lab_1.Core.Entities;

public class Enterprise : IEnterprise
{
    public string LegalName { get; set; }
    public string UNP { get; set; }
    public string BIC { get; set; }
    public string LegalAddress { get; set; }
    public Bank Bank { get; set; }
    public List<EnterpriseAccount> Accounts {get;}
    public void ApplyForSalaryProject(Bank bank){}
    public void RequestIntercompanyTransfer(decimal amount, Enterprise targetEnterprise){}
}