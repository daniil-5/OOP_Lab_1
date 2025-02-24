using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_Lab_1.UI
{
    public partial class CloseAccountPage : ContentPage, IQueryAttributable
    {
        private string SelectedBank { get; set; } = "Unknown";
        private string BankId { get; set; } = "Unknown";
        private IClientActions _clientActions;
        private User CurrentUser;
        private readonly IAccountService _accountService;

        public CloseAccountPage(IAccountService accountService)
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
            
            _ = LoadUserAccountsAsync();
        }
        
        private async Task LoadUserAccountsAsync()
        {
            try
            {
                string tableName = BankId + "_UserAccounts";
                var accounts = await _accountService.GetAccountsByEmailAsync(tableName, CurrentUser.Email);

                if (accounts != null && accounts.Any())
                {
                    AccountsPicker.ItemsSource = accounts.Select(account => account.ToString()).ToList();
                }
                else
                {
                    await DisplayAlert("No Accounts", "You have no accounts to close.", "OK");
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load accounts: {ex.Message}", "OK");
            }
        }

        private async void OnCloseAccountClicked(object sender, EventArgs e)
        {
            string selectedAccount = AccountsPicker.SelectedItem as string;

            if (string.IsNullOrEmpty(selectedAccount))
            {
                await DisplayAlert("Selection Required", "Please select an account to close.", "OK");
                return;
            }
            
            bool confirm = await DisplayAlert("Confirm Closure", 
                $"Are you sure you want to close account {selectedAccount}?", "Yes", "No");

            if (confirm)
            {
                string tableName = BankId + "_UserAccounts";
                int startIndex = selectedAccount.IndexOf("Account number:") + "Account number:".Length;
                int endIndex = selectedAccount.IndexOf(";", startIndex);
                string accountNumber = selectedAccount.Substring(startIndex, endIndex - startIndex).Trim();
                bool isClosed = await _accountService.DeleteAccountAsync(tableName, accountNumber);

                if (isClosed)
                {
                    await DisplayAlert("Success", "Account closed successfully!", "OK");
                    await LoadUserAccountsAsync();
                }
                else
                {
                    await DisplayAlert("Error", "Failed to close the account. Please try again.", "OK");
                }
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            AccountsPicker.SelectedIndex = -1;
            await Shell.Current.GoToAsync("..");
        }
    }
}