using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;
using OOP_Lab_1.Core.Services;

namespace OOP_Lab_1.UI;

public partial class LoginPage : ContentPage, IQueryAttributable
{
    private string SelectedBank { get; set; } = "Unknown";
    private string BankId { get; set; } = "Unknown";
    private readonly IDatabaseService _databaseService;

    public LoginPage(IDatabaseService databaseService)
    {
        InitializeComponent();
        _databaseService = databaseService;
    }
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("BankName"))
        {
            SelectedBank = query["BankName"] as string;
            BankLabel.Text = SelectedBank;
        }
        if (query.ContainsKey("BankId"))
        {
            BankId = query["BankId"] as string;
        }
    }
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text?.Trim() ?? string.Empty;
        string password = PasswordEntry.Text?.Trim() ?? string.Empty;
        
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Validation Error", "Please enter both email and password.", "OK");
            return;
        }
        
        var user = await AuthenticateUserAsync(email, password);
        if (user != null)
        {
            await DisplayAlert("Success", $"Welcome, {user.FullName}!", "OK");
            await Shell.Current.GoToAsync("main", true, new Dictionary<string, object>
            {
                { "BankName", SelectedBank },
                { "BankId", BankId },
                {"CurrentUser", user}
            });
        }
        else
        {
            await DisplayAlert("Error", "Invalid email or password.", "OK");
        }
    }

    private async Task<User?> AuthenticateUserAsync(string email, string password)
    {
        var users = await _databaseService.GetUserByEmail(email, BankId);
        if (users == null)
            return null;
        else
            return users;
    }

    private async void OnRegisterTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("registration", true, new Dictionary<string, object>
        {
            { "BankName", SelectedBank },
            { "BankId", BankId }
        });
    }
}