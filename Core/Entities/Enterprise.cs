using OOP_Lab_1.Core.Interfaces;
using SQLite;
namespace OOP_Lab_1.Core.Entities;

public class Enterprise : IEnterprise
{
    public int Id { get; set; }
    public string LegalName { get; set; }
    public string Type { get; set; }
    public string UNP { get; set; }
    public string BIC { get; set; }
    public string LegalAddress { get; set; }
}