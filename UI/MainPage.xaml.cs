using System.Reflection;
using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;
using OOP_Lab_1.UI;

namespace OOP_Lab_1;

public partial class MainPage : ContentPage, IQueryAttributable
{
    private string _selectedBank;
    private string _bankId;
    
    private User _currentUser;


    
    public MainPage()
    {
        InitializeComponent();
    }

    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("BankName"))
        {
            _selectedBank = query["BankName"] as string;
        }
        if (query.ContainsKey("BankId"))
        {
            _bankId = query["BankId"] as string;
        }
        if (query.ContainsKey("CurrentUser"))
        {
            _currentUser = query["CurrentUser"] as User;
        }
        
        StackLayout.Clear();
        CreateRoleButtons();
    }
    
    private async void CreateRoleButtons()
    {
        if (_currentUser.Role == 0)
        {
            CreateNavigationButton("Open new Account", "openAccount");
            CreateNavigationButton("Close account", "closeAccount");
            CreateNavigationButton("Apply for Loan", "getLoan");
            // CreateNavigationButton("Apply for Salary Project", "applySalaryProject");
            CreateNavigationButton("Transfer to account", "transfer");
            CreateNavigationButton("Withdraw Cash", "withdrawCash");
            CreateNavigationButton("Refill Balance", "refillBalance");
        }
        
        if (_currentUser.Role == 2)
        {
            CreateNavigationButton("Approve Loan", "LoanApprovement");
            CreateNavigationButton("Approve Registaration", "UserApprovement");
            // CreateNavigationButton("Cancel External Transactions", nameof(CancelExternalTransactionPage));
        }
        
        // if (_currentUser.Role == 1)
        // {
        //     CreateNavigationButton("View Transaction Statistics", nameof(TransactionStatisticsPage));
        //     CreateNavigationButton("Confirm Salary Project", nameof(ConfirmSalaryProjectPage));
        // }
        //
        // if (_currentUser.Role == 3)
        // {
        //     CreateNavigationButton("Submit Salary Project Documents", nameof(SubmitDocsForSalaryProjectPage));
        //     CreateNavigationButton("Request Transfer", nameof(RequestFundTransferPage));
        // }
        //
        // if (_currentUser.Role == 4)
        // {
        //     CreateNavigationButton("View Logs", nameof(ViewLogsPage));
        //     CreateNavigationButton("Cancel User Actions", nameof(CancelUserActionsPage));
        // }

        // Logout Button
        Button logoutButton = new Button
        {
            Text = "Logout",
            BackgroundColor = Color.FromArgb("#ADD8E6")
        };
        logoutButton.Clicked += OnLogoutClicked;
        StackLayout.Children.Add(logoutButton);
    }
    private void CreateNavigationButton(string label, string targetPage)
    {
        var button = new Button { Text = label };
        button.Clicked += async (sender, args) =>
        {
            await Shell.Current.GoToAsync(targetPage, true, new Dictionary<string, object>
            {
                {"BankName", _selectedBank},
                { "BankId", _bankId },
                {"CurrentUser", _currentUser}
            });
        };
        StackLayout.Children.Add(button);
    }
    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("banks");
    }
}