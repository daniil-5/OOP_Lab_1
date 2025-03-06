using OOP_Lab_1.Core.Interfaces;
using OOP_Lab_1.UI.Models;

namespace OOP_Lab_1.UI;

public partial class RegistrationApprovalPage : ContentPage, IQueryAttributable
{
    private readonly UserApprovalViewModel _viewModel;

    public RegistrationApprovalPage(IUserService userService)
    {
        InitializeComponent();
        _viewModel = new UserApprovalViewModel(userService);
        BindingContext = _viewModel;
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("BankId"))
        {
            var bankId = query["BankId"] as string;
            _viewModel.ApplyBic(bankId);
        }
    }
}