using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOP_Lab_1.Core.Interfaces;
using OOP_Lab_1.Core.Services;
using OOP_Lab_1.UI.Models;

namespace OOP_Lab_1.UI;

public partial class ConnectedToSalaryPage : ContentPage
{
    
    public ConnectedToSalaryPage()
    {
        InitializeComponent();
    }
    
    private readonly ConnectedToSalaryWorkersViewModel _viewModel;

    public ConnectedToSalaryPage(IUserService usertService, ISalaryProjectService salaryProjectService)
    {
        InitializeComponent();
        _viewModel = new ConnectedToSalaryWorkersViewModel(usertService, salaryProjectService);
        BindingContext = _viewModel;
        _viewModel.LoadTransactions();
    }
}