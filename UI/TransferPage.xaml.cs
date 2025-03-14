using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;
using OOP_Lab_1.UI.Models;

namespace OOP_Lab_1.UI;

public partial class TransferPage : ContentPage, IQueryAttributable
{
    private string _bankId = "Unknown";
    private User _currentUser;
    private readonly IAccountService _accountService;
    private TransferViewModel _viewModel;
    
    public TransferPage(IAccountService accountService)
    {
        InitializeComponent();
        _accountService = accountService;
        _viewModel = new TransferViewModel(accountService);
        BindingContext = _viewModel;
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("BankId"))
        {
            _bankId = query["BankId"] as string ?? "Unknown";
        }
        if (query.ContainsKey("CurrentUser"))
        {
            _currentUser = query["CurrentUser"] as User;
        }

        if (_currentUser != null)
        {
            _viewModel.ApplyUserEmail(_currentUser.Email);
            _viewModel.ApplyBic(_bankId);
            _ = _viewModel.LoadAccountsAsync();
        }
    }
}