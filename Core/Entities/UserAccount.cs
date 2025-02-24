
using OOP_Lab_1.Core.Interfaces;
using SQLite;
namespace OOP_Lab_1.Core.Entities;

public class UserAccount : Account
{
    public UserAccount(string accountNumber, double balance, bool isBlocked, bool isFrozen, string userEmail)
    {
        AccountNumber = accountNumber;
        Balance = balance;
        IsBlocked = isBlocked;
        IsFrozen = isFrozen;
        UserEmail = userEmail;
    }

    public override string ToString()
    {
        string output =
            $"Account number:{AccountNumber}; Balance:{Balance:C}; IsBlocked:{IsBlocked.ToString()}; IsFrozen:{IsFrozen.ToString()}";
        return output;
    }

    public string UserEmail { get; set; }
    }