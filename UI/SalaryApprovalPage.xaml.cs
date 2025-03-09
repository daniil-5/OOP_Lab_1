using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOP_Lab_1.Core.Services;
using OOP_Lab_1.UI.Models;

namespace OOP_Lab_1.UI;

public partial class SalaryApprovalPage : ContentPage
{
    public SalaryApprovalViewModel _viewModel { get; set; }
    public SalaryApprovalPage(ISalaryProjectService salaryProjectService)
    {
        InitializeComponent();
        _viewModel = new SalaryApprovalViewModel(salaryProjectService);
        BindingContext = _viewModel;
    }
}