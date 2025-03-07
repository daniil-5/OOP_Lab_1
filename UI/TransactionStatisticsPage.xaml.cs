using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOP_Lab_1.Core.Interfaces;
using OOP_Lab_1.UI.Models;

namespace OOP_Lab_1.UI;

public partial class TransactionStatisticsPage : ContentPage
{
    private readonly TransactionStatisticsViewModel _viewModel;

    public TransactionStatisticsPage(IAccountService accountService)
    {
        InitializeComponent();
        _viewModel = new TransactionStatisticsViewModel(accountService);
        BindingContext = _viewModel;
        _viewModel.LoadTransactions();
    }
    
}