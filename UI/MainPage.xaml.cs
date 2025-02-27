using System.Reflection;
using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;
using OOP_Lab_1.UI;

namespace OOP_Lab_1;

public partial class MainPage : ContentPage, IQueryAttributable
{
    private string SelectedBank { get; set; } = "Unknown";
    private string BankId { get; set; } = "Unknown";
    
    private User CurrentUser;
    private User RoledUser;


    
    public MainPage()
    {
        InitializeComponent();
    }

    private void CreateTypeUser(int role)
    {
        switch(CurrentUser.Role)
        {
            case 0:
                RoledUser = new Client(CurrentUser);
                break;
            case 1:
                RoledUser = new Operator(CurrentUser);
                break;
            case 2:
                RoledUser = new Manager(CurrentUser);
                break;
            case 3:
                RoledUser = new EnterpriseSpecialist(CurrentUser);
                break;
            case 4:
                RoledUser = new Admin(CurrentUser);
                break;
            default:
                throw new InvalidOperationException("Unknown role");
        }
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("BankName"))
        {
            SelectedBank = query["BankName"] as string;
        }
        if (query.ContainsKey("BankId"))
        {
            BankId = query["BankId"] as string;
        }
        if (query.ContainsKey("CurrentUser"))
        {
            CurrentUser = query["CurrentUser"] as User;
        }
        
        CreateTypeUser(CurrentUser.Role);
        StackLayout.Clear();
        CreateRoleButtons();
    }
    
    private async void CreateRoleButtons()
    {
        if (RoledUser is IClientActions clientActions)
        {
            CreateNavigationButton("Open new Account", "openAccount", clientActions);
            CreateNavigationButton("Close account", "closeAccount", clientActions);
            CreateNavigationButton("Apply for Loan", "getLoan", clientActions);
            // CreateNavigationButton("Apply for Salary Project", nameof(ApplySalaryProjectPage), clientActions);
        }
        //
        if (RoledUser is IManagerActions managerActions)
        {
            CreateNavigationButton("Approve Loan", "LoanApprovement", managerActions);
            // CreateNavigationButton("Cancel External Transactions", nameof(CancelExternalTransactionPage), managerActions);
        }
        
        // if (RoledUser is IOperatorActions operatorActions)
        // {
        //     CreateNavigationButton("View Transaction Statistics", nameof(TransactionStatisticsPage), operatorActions);
        //     CreateNavigationButton("Confirm Salary Project", nameof(ConfirmSalaryProjectPage), operatorActions);
        // }
        //
        // if (RoledUser is IEnterpriseSpecialistActions enterpriseSpecialistActions)
        // {
        //     CreateNavigationButton("Submit Salary Project Documents", nameof(SubmitDocsForSalaryProjectPage), enterpriseSpecialistActions);
        //     CreateNavigationButton("Request Transfer", nameof(RequestFundTransferPage), enterpriseSpecialistActions);
        // }
        //
        // if (RoledUser is IAdminActions adminActions)
        // {
        //     CreateNavigationButton("View Logs", nameof(ViewLogsPage), adminActions);
        //     CreateNavigationButton("Cancel User Actions", nameof(CancelUserActionsPage), adminActions);
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
    private void CreateNavigationButton<T>(string label, string targetPage, T roleObject)
    {
        var button = new Button { Text = label };
        button.Clicked += async (sender, args) =>
        {
            await Shell.Current.GoToAsync(targetPage, true, new Dictionary<string, object>
            {
                { "BankName", SelectedBank },
                { "BankId", BankId },
                { "RoleData", roleObject }, // Sending only the relevant interface object
                {"CurrentUser", CurrentUser}
            });
        };
        StackLayout.Children.Add(button);
    }
    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("banks");
    }
}