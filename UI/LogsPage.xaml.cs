using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOP_Lab_1.Core.Services;
using OOP_Lab_1.UI.Models;

namespace OOP_Lab_1.UI;

public partial class LogsPage : ContentPage
{
    private ILogService _logService;
    private LogsViewModel _viewModel;
    public LogsPage(ILogService logService)
    {
        InitializeComponent();
        _logService = logService;
        _viewModel = new LogsViewModel(_logService);
        BindingContext = _viewModel;
    }
}