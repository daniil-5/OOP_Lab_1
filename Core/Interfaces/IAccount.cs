using OOP_Lab_1.Core.Entities;

namespace OOP_Lab_1.Core.Interfaces;

public interface IAccount
{
    public string AccountNumber { get;}
    public double Balance { get; }
    bool IsBlocked { get;}
    bool IsFrozen { get;}
    string BIC { get; set; }
    void Deposit(double amount);
    bool Withdraw(double amount);
    bool Transfer(double amount, IAccount target);
    
    void Block();
    void Unblock();
    void Freeze();
    void Unfreeze();
    
}