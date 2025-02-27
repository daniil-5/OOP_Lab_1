using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;
using Microsoft.Maui.Controls;
using OOP_Lab_1.UI.Models;

namespace OOP_Lab_1.UI;

public partial class LoanApprovalPage : ContentPage, IQueryAttributable
{
    private readonly LoanApprovalViewModel _viewModel;

    public LoanApprovalPage(ILoanService loanService)
    {
        InitializeComponent();
        _viewModel = new LoanApprovalViewModel(loanService);
        BindingContext = _viewModel;
    }

    // Receive the query attributes, typically from navigation
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("BankId"))
        {
            var bankId = query["BankId"] as string;
            _viewModel.ApplyBankId(bankId); // Apply Bank ID to ViewModel
        }
    }
}