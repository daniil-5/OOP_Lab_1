using OOP_Lab_1.Core.Entities;
namespace OOP_Lab_1.Core.Interfaces;

public interface ILoan
{
    double Amount { get; }
    int DurationMonths { get; }
    double InterestRate { get; }
    bool Approved { get; }
    void Approve();
    void Reject();
    User Borrower { get; set; }
    IBank IssuingBank { get; set; }
}