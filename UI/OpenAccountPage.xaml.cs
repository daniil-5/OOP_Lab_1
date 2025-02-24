using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;

namespace OOP_Lab_1.UI;

public partial class OpenAccountPage : ContentPage, IQueryAttributable
{
    private string SelectedBank { get; set; } = "Unknown";
    private string BankId { get; set; } = "Unknown";
    private IClientActions _clientActions;
    private User CurrentUser;
    private readonly IAccountService _accountService;
    public OpenAccountPage(IAccountService accountService)
    {
        InitializeComponent();
        _accountService = accountService;
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
        if (query.ContainsKey("RoleData") && query["RoleData"] is IClientActions clientActions)
        {
            _clientActions = clientActions;
        }
        if (query.ContainsKey("CurrentUser"))
        {
            CurrentUser = query["CurrentUser"] as User;
        }
    }
    private async void OnOpenAccountClicked(object sender, EventArgs e)
    {
        string selectedAccountType = AccountTypePicker.SelectedItem as string;
        string initialDepositText = InitialDepositEntry.Text;
            
        if (string.IsNullOrWhiteSpace(selectedAccountType) || string.IsNullOrWhiteSpace(initialDepositText))
        {
            await DisplayAlert("Error", "Please select an account type and enter an initial deposit.", "OK");
            return;
        }

        if (!double.TryParse(initialDepositText, out double initialDeposit) || initialDeposit < 0)
        {
            await DisplayAlert("Error", "Please enter a valid deposit amount.", "OK");
            return;
        }
        
        UserAccount newAccount = new UserAccount(GenerateAccountNumber(), Double.Parse(initialDepositText), false, false, CurrentUser.Email);
        
        string tableName = BankId + "_UserAccounts";
        
        bool isCreated = await _accountService.AddAccountAsync(tableName, newAccount);
        if (!isCreated)
        {
            await DisplayAlert("Error", "Failed to open account. Please try again.", "OK");
            return;
        }
        await DisplayAlert("Success", $"Account '{selectedAccountType}' opened with {initialDeposit:C}!", "OK");
        ClearForm();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        ClearForm();
        await Shell.Current.GoToAsync("..");
    }
    private void ClearForm()
    {
        AccountTypePicker.SelectedIndex = -1;
        InitialDepositEntry.Text = string.Empty;
    }
    public static string GenerateAccountNumber()
    {
        Guid guid = Guid.NewGuid();
        return guid.ToString("N").Substring(0, 16); // Take first 16 characters of GUID
    }
    
}