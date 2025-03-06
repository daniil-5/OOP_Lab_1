using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Generators;
using OOP_Lab_1.Core.Interfaces;

namespace OOP_Lab_1.UI;

public partial class GetLoanPage : ContentPage, IQueryAttributable
{
    private string BankId { get; set; } = "Unknown";
    private User CurrentUser;
    private ILoanService _loanService;
    
    public GetLoanPage(ILoanService loanService)
    {
        InitializeComponent();
        _loanService = loanService;
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("BankId"))
        {
            BankId = query["BankId"] as string;
        }
        if (query.ContainsKey("CurrentUser"))
        {
            CurrentUser = query["CurrentUser"] as User;
        }
    }
    private void UpdatePercentage()
    {
        if (LoanTypePicker.SelectedIndex == -1 || 
            PercentageTypePicker.SelectedIndex == -1 || 
            LoanDurationPicker.SelectedIndex == -1)
        {
            CurrentPercent.TextColor = Colors.Red;
            CurrentPercent.Text = "Select all options for loan percentage";
            return;
        }

        string loanType = LoanTypePicker.SelectedItem.ToString();
        string percentType = PercentageTypePicker.SelectedItem.ToString();
        int duration = int.Parse(LoanDurationPicker.SelectedItem.ToString());
        double interestRate = CalculateInterestRate(loanType, percentType, duration);
        CurrentPercent.TextColor = Colors.Orange;
        CurrentPercent.Text = $"{interestRate}";
    }
    private double CalculateInterestRate(string loanType, string percentType, int duration)
    {
        double baseRate = 0;

        if (loanType == "Credit")
        {
            baseRate = percentType == "Individual" ? 5.2 : 7.5;
        }
        else if (loanType == "Installment Plan")
        {
            baseRate = percentType == "Individual" ? 4.3 : 5.5;
        }
        
        if (duration <= 6) baseRate += 0.5;
        else if (duration <= 12) baseRate += 1.0;
        else if (duration <= 24) baseRate += 1.5;
        else baseRate += 2.0;

        return baseRate;
    }

    private void OnLoanTypeChanged(object sender, EventArgs e)
    {
        UpdatePercentage();
    }

    private void OnPercentTypeChanged(object sender, EventArgs e)
    {
        UpdatePercentage();
    }
    private async void OnSubmitLoanClicked(object sender, EventArgs e)
    {
        if (LoanTypePicker.SelectedIndex == -1 ||
            LoanDurationPicker.SelectedIndex == -1 ||
            PercentageTypePicker.SelectedIndex == -1 ||
            !double.TryParse(LoanAmountEntry.Text, out double amount) ||
            string.IsNullOrWhiteSpace(LoanReasonEditor.Text))
        {
            await DisplayAlert("Validation Error", "Please fill in all fields correctly.", "OK");
            return;
        }
        
        string loanType = LoanTypePicker.SelectedItem.ToString();
        int duration = int.Parse(LoanDurationPicker.SelectedItem.ToString());
        string percentageType = PercentageTypePicker.SelectedItem.ToString();
        string reason = LoanReasonEditor.Text;

        Loan newLoan = new Loan
        {
            LoanId = IdGenerator.GenerateId(8),
            TypeOfLoan = LoanTypePicker.SelectedIndex,
            TypeOfPercent = PercentageTypePicker.SelectedIndex,
            Percent = double.Parse(CurrentPercent.Text),
            Amount = double.Parse(LoanAmountEntry.Text),
            DurationMonths = int.Parse(LoanDurationPicker.SelectedItem.ToString()),
            Purpose = LoanReasonEditor.Text,
            UserEmail = CurrentUser.Email,
            Approved = false
        };
        Console.WriteLine("Saving loan");
        bool success = await _loanService.AddLoanAsync(newLoan, BankId);
        if (success)
        {
            await DisplayAlert("Success", "Loan application submitted successfully!", "OK");
            ClearForm();
        }
        else
        {
            await DisplayAlert("Error", "Loan application failed. Try again.", "OK");
        }
    }
    private void ClearForm()
    {
        LoanTypePicker.SelectedIndex = -1;
        LoanDurationPicker.SelectedIndex = -1;
        PercentageTypePicker.SelectedIndex = -1;
        LoanReasonEditor.Text = "";
        LoanAmountEntry.Text = "";
    }
    
}
