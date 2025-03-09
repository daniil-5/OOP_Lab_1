
using OOP_Lab_1.Core.Interfaces;
using OOP_Lab_1.UI.ViewModels;
using Microsoft.Maui.Controls;
using OOP_Lab_1.UI.Models;

namespace OOP_Lab_1.UI
{
    public partial class RegistrationPage : ContentPage, IQueryAttributable
    {
        private RegistrationViewModel _viewModel;
        public RegistrationPage(IUserService userService)
        {
            InitializeComponent();
            _viewModel = new RegistrationViewModel(userService);
            BindingContext = _viewModel;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("BankId") && BindingContext is RegistrationViewModel viewModel)
            {
                viewModel.BankId = query["BankId"] as string;
            }
        }
    }
}