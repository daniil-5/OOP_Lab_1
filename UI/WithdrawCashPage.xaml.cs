using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;
using OOP_Lab_1.UI.Models;
using OOP_Lab_1.UI.ViewModels;

namespace OOP_Lab_1.UI
{
    public partial class WithdrawCashPage : ContentPage, IQueryAttributable
    {
        private string _bankId = "Unknown";
        private User _currentUser;
        private WithdrawViewModel _viewModel;

        public WithdrawCashPage(IAccountService accountService)
        {
            InitializeComponent();
            _viewModel = new WithdrawViewModel(accountService);
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
                _viewModel.ApplyBIC(_bankId);
                _ = _viewModel.LoadAccountsAsync();
            }
        }
    }
}