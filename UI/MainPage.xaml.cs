using System.Reflection;
using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;

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
        CreateRoleButtons();
    }
    
    private async void CreateRoleButtons()
    {
        if (RoledUser is IClientActions clientActions)
        {
            CreateButton("Register", clientActions.Register);
            CreateButton("Open new Account", clientActions.OpenAccount);
            CreateButton("Close account", clientActions.CloseAccount);
            CreateButton("Apply for Loan", clientActions.ApplyForLoan);
            CreateButton("Apply for Salary Project", clientActions.ApplyForSalaryProject);
        }
        
        // Check for IManagerActions methods if the current role is Manager
        if (RoledUser is IManagerActions managerActions)
        {
            CreateButton("Approve Loan", managerActions.ApproveLoan);
            CreateButton("Cancel External Transactions", managerActions.CancelExternalTransaction);
        }
        // Check for IOperatorActions methods if the current role is Operator
        if (RoledUser is IOperatorActions operatorActions)
        {
            CreateButton("View Transaction Statistics", operatorActions.ViewTransactions);
            CreateButton("Confirm Salary Project", operatorActions.ConfirmSalaryProject);
        }
        
        // Check for IEnterpriseSpecialistActions methods if the current role is Enterprise Specialist
        if (RoledUser is IEnterpriseSpecialistActions enterpriseSpecialistActions)
        {
            CreateButton("Submit Salary Project Documents to Operator", enterpriseSpecialistActions.SubmitDocsForSalaryProject);
            CreateButton("Request Transfer to External Enterprise or Worker", enterpriseSpecialistActions.RequestFundTransfer);
        }
        
        // Check for IAdminActions methods if the current role is Admin
        if (RoledUser is IAdminActions adminActions)
        {
            CreateButton("View Logs", adminActions.ViewLogs);
            CreateButton("Cancel User Actions", adminActions.CancelUserActions);
        }

        // Logout Button
        Button logoutButton = new Button
        {
            Text = "Logout",
            BackgroundColor = Color.FromArgb("#ADD8E6")
        };
        logoutButton.Clicked += OnLogoutClicked;
        StackLayout.Children.Add(logoutButton);
    }
    private void CreateButton(string label, Action action)
    {
        var button = new Button { Text = label };
        button.Clicked += (sender, args) => action();
        StackLayout.Children.Add(button);
    }
    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        
        await Shell.Current.GoToAsync("banks");
    }
}