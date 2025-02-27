using OOP_Lab_1.Core.Entities;
namespace OOP_Lab_1.Core.Interfaces;

public interface ILoan
{ 
    string LoanId { get; set; }
    int TypeOfLoan { get; set; }
    int TypeOfPercent { get; set; }
    double Percent { get; set; }
    double Amount { get; }
    int DurationMonths { get; }
    string Purpose { get; set; }
    string UserEmail { get; set; }
    bool Approved { get; }
    void Approve();
    void Reject();
}