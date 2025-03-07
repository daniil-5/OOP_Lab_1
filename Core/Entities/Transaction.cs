namespace OOP_Lab_1.Core.Entities;

public class Transaction
{
    public string FromAccountId { get; set; }
    public string ToAccountId { get; set; }
    public double Amount { get; set; }
    public DateTime Date { get; set; }
}