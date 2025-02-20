namespace OOP_Lab_1.Core.Interfaces;

public interface IEnterprise
{
    string LegalName { get; set; }
    string TaxNumber { get; set; }
    string BankBIC { get; set; }
    string LegalAddress { get; set; }
    IBank Bank { get; set; }

    void SubmitPayrollProjectRequest();
    void RequestIntercompanyTransfer(decimal amount, IEnterprise targetEnterprise);
}