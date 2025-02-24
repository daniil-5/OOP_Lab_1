using OOP_Lab_1.Core.Entities;

namespace OOP_Lab_1.Core.Interfaces;

public interface IClientActions
{
    //Task<bool> OpenAccountAsync(UserAccount account, string tableName);
    void CloseAccount();
    void ApplyForLoan();
    void ApplyForSalaryProject();
}