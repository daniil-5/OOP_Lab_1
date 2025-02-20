namespace OOP_Lab_1.Core.Interfaces;

public interface IAccount
{
    string AccountNumber { get; }
    double Balance { get; }
    void Deposit(double amount);
    void Withdraw(double amount);
    void Transfer(IAccount target, double amount);
    void Freeze();
    void Unfreeze();
}