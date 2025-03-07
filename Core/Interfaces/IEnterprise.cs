using OOP_Lab_1.Core.Entities;

namespace OOP_Lab_1.Core.Interfaces;

public interface IEnterprise
{
    string LegalName { get; set; }
    string Type { get; set; }
    string UNP { get; set; }
    string BIC { get; set; }
    string LegalAddress { get; set; }
}