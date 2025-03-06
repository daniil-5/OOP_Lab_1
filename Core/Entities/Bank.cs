using OOP_Lab_1.Core.Interfaces;
using SQLite;
namespace OOP_Lab_1.Core.Entities;

public abstract class Bank
{
    public string Name { get; set; }
    public string BIC { get; set; }
    
}