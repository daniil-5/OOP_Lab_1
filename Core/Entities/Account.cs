using OOP_Lab_1.Core.Interfaces;

namespace OOP_Lab_1.Core.Entities;

public class Account : IAccount
{
    public Bank Bank { get; set; }
    public string AccountNumber { get; private set; }
    public double Balance { get; private set; }
    public bool IsBlocked { get; private set; }
    public bool IsFrozen { get; private set; }

    public void Deposit(double amount)
    {
        if (IsBlocked)
        {
            Console.WriteLine($"Account {AccountNumber} is blocked");
            return;
        }
        else if (IsFrozen)
        {
            Console.WriteLine($"Account {AccountNumber} is frozen");
            return;
        }

        Balance += amount;
        Console.WriteLine($"Deposited {amount} to account {AccountNumber}");
    }

    public bool Withdraw(double amount)
    {
        if (Balance >= amount)
        {
            Balance -= amount;
            Console.WriteLine($"Withdrew {amount} from account {AccountNumber}");
            return true;
        }
        else
        {
            Console.WriteLine($"Insufficient funds in account {AccountNumber}");
            return false;
        }
    }

    public bool Transfer(double amount, IAccount target)
    {
        if (IsBlocked)
        {
            Console.WriteLine($"Account {AccountNumber} is blocked");
            return false;
        }
        else if (IsFrozen)
        {
            Console.WriteLine($"Account {AccountNumber} is frozen");
            return false;
        }

        if (Withdraw(amount))
        {
            target.Deposit(amount);
            Console.WriteLine($"Transferred {amount} from account {AccountNumber} to account {target.AccountNumber}");
            return true;
        }
        else
        {
            Console.WriteLine($"Transfer failed from account {AccountNumber} to account {target.AccountNumber}");
            return false;
        }
    }

    public void Block()
    {
        IsBlocked = true;
    }

    public void Unblock()
    {
        IsBlocked = false;
    }

    public void Freeze()
    {
        IsFrozen = true;
    }

    public void Unfreeze()
    {
        IsFrozen = false;
    }
}