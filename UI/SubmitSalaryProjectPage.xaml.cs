using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;
using OOP_Lab_1.Core.Services;
using OOP_Lab_1.UI.Models;

namespace OOP_Lab_1.UI;

public partial class SubmitSalaryProjectPage : ContentPage, IQueryAttributable
{
    private User _currentUser;
    public SalaryProjectViewModel _viewModel { get; set; }

    public SubmitSalaryProjectPage(ISalaryProjectService salaryProjectService, IUserService userService)
    {
        InitializeComponent();
        _viewModel = new SalaryProjectViewModel(salaryProjectService, userService);
        BindingContext = _viewModel;
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("CurrentUser"))
        {
            _currentUser = query["CurrentUser"] as User;
        }
        _viewModel.ApplyUser(_currentUser);
    }
    
}