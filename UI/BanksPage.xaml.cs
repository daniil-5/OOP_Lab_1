using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOP_Lab_1.Core.Interfaces;

namespace OOP_Lab_1.UI;

public partial class BanksPage : ContentPage
{
    private readonly IBanksService _banksService;
    public BanksPage(IBanksService banksService)
    {
        InitializeComponent();
        _banksService = banksService;
        LoadBanks(_banksService);
    }
    private async void LoadBanks(IBanksService banksService)
    {
        try
        {
            BanksLayout.Children.Clear();
            var banks = await banksService.GetBanksAsync();

            foreach (var bank in banks)
            {
                await banksService.CreateBankTableAsync(bank.BankId);
                string tableName = bank.BankId + "_UserAccounts";
                await _banksService.CreateAccountTableAsync(tableName);
                var bankButton = new Button
                {
                    Text = bank.BankName,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Margin = new Thickness(0, 0, 0, 10),
                    FontSize = 16,
                    BackgroundColor = Color.FromArgb("#ADD8E6"), // Light blue background
                    TextColor = Colors.Black
                };
                
                bankButton.Clicked += async (s, e) =>
                {
                    await Shell.Current.GoToAsync("login", true, new Dictionary<string, object>
                    {
                        { "BankName", bank.BankName },
                        { "BankId", bank.BankId }
                    });
                };
                BanksLayout.Children.Add(bankButton);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load banks: {ex.Message}", "OK");
        }
    }
}