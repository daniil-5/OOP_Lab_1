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

public partial class ApplyForSalaryPage : ContentPage, IQueryAttributable
{
    private ApplySalaryProjectViewModel _viewModel { get; set; }
    private ISalaryProjectService _salaryProjectService;
    private IUserService _userService;
    private User _currentUser;
    
    public ApplyForSalaryPage(ISalaryProjectService salaryProjectService, IUserService userService)
    {
        InitializeComponent();
        _salaryProjectService = salaryProjectService;
        _userService = userService;
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("CurrentUser"))
        {
            _currentUser = query["CurrentUser"] as User;
        }
        _viewModel = new ApplySalaryProjectViewModel(_salaryProjectService, _userService, _currentUser);
        BindingContext = _viewModel;
    }
}