using System.Collections.ObjectModel;
using System.Windows.Input;
using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;

namespace OOP_Lab_1.UI.Models;

public class TransactionStatisticsViewModel : BaseViewModel
{
    private readonly IAccountService _accountService;

    public ObservableCollection<Transaction> Transactions { get; set; }
    
    public ICommand UndoCommand { get; }

    public TransactionStatisticsViewModel(IAccountService accountService)
    {
        _accountService = accountService;
        Transactions = new ObservableCollection<Transaction>();
        UndoCommand = new Command<Transaction>(async (transaction) => await UndoTransaction(transaction));
    }
    
    public async void LoadTransactions()
    {
        var transactions = await _accountService.GetAllTransactionsAsync();
        Transactions.Clear();
        foreach (var transaction in transactions)
        {
            Transactions.Add(transaction);
        }
    }
    
    // Reject Transaction
    private async Task UndoTransaction(Transaction transaction)
    {
        // await _accountService.DeleteUserAsync(user.Id);
        // TO DO implement the undo of transaction and return money to account
        Console.WriteLine("Rejecting transaction");
        Transactions.Remove(transaction);
        //LoadTransactions();
    }
}