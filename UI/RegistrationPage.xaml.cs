// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
//
// using System.Net.Mail;
// using System.Threading.Tasks;
// using OOP_Lab_1.Core.Entities;
// using OOP_Lab_1.Core.Interfaces;
// using OOP_Lab_1.Core.Services;
//
// namespace OOP_Lab_1.UI
// {
//     public partial class RegistrationPage : ContentPage, IQueryAttributable
//     {
//         private string BankId { get; set; } = "Unknown";
//         
//         private readonly IUserService _userService;
//
//         public RegistrationPage(IUserService userService)
//         {
//             InitializeComponent();
//             _userService = userService;
//         }
//         public void ApplyQueryAttributes(IDictionary<string, object> query)
//         {
//             if (query.ContainsKey("BankId"))
//             {
//                 BankId = query["BankId"] as string;
//             }
//         }
//
//         private async void OnRegisterClicked(object sender, EventArgs e)
//         {
//             try
//             { 
//                 string fullName = FullNameEntry.Text?.Trim() ?? string.Empty;
//                 string passportNumber = PassportNumberEntry.Text?.Trim() ?? string.Empty;
//                 string identificationNumber = IdentificationNumberEntry.Text?.Trim() ?? string.Empty;
//                 string phone = PhoneEntry.Text?.Trim() ?? string.Empty;
//                 string email = EmailEntry.Text?.Trim() ?? string.Empty;
//                 string password = PasswordEntry.Text?.Trim() ?? string.Empty;
//                 int selectedRole = RolePicker.SelectedIndex;
//                 if (string.IsNullOrEmpty(fullName) || 
//                     string.IsNullOrEmpty(passportNumber) || 
//                     string.IsNullOrEmpty(identificationNumber) || 
//                     string.IsNullOrEmpty(phone) || 
//                     string.IsNullOrEmpty(email) || 
//                     string.IsNullOrEmpty(password) ||
//                     selectedRole == -1)
//                 {
//                     await DisplayAlert("Validation Error", "Please fill in all fields.", "OK");
//                     return;
//                 }
//
//                 if (!IsValidEmail(email))
//                 {
//                     await DisplayAlert("Validation Error", "Please enter a valid email address.", "OK");
//                     return;
//                 }
//
//                 if (password.Length < 6)
//                 {
//                     await DisplayAlert("Validation Error", "Password must be at least 6 characters long.", "OK");
//                     return;
//                 }
//                 
//                 bool userExists = await _userService.UserExistsByEmailAsync(email, BankId);
//                 if (userExists)
//                 {
//                     await DisplayAlert("Error", "A user with this email already exists.", "OK");
//                     return;
//                 }
//                 
//                 var newUser = new User
//                 {
//                     FullName = fullName,
//                     PassportNumber = passportNumber,
//                     IdentificationNumber = identificationNumber,
//                     Phone = phone,
//                     Email = email,
//                     Password = password, // Note: Hash the password before saving in a real app
//                     Role = selectedRole
//                 };
//
//                 bool isAdded = await _userService.RegisterUserAsync(newUser, BankId);
//                 if (isAdded)
//                 {
//                     await DisplayAlert("Success", $"User {fullName} registered as {GetStringRole(selectedRole)}.", "OK");
//                     ClearForm();
//                     await Shell.Current.GoToAsync("..");
//                 }
//                 else
//                 {
//                     await DisplayAlert("Error", "Failed to register the user. Please try again.", "OK");
//                 }
//             }
//             catch (Exception exception)
//             {
//                 Console.WriteLine(exception);
//                 await DisplayAlert("Validation Error", $"{exception}", "OK");
//                 throw;
//             }
//             
//         }
//
//         private bool IsValidEmail(string email)
//         {
//             try
//             {
//                 var addr = new MailAddress(email);
//                 return addr.Address == email;
//             }
//             catch
//             {
//                 return false;
//             }
//         }
//
//         private void ClearForm()
//         {
//             FullNameEntry.Text = string.Empty;
//             PassportNumberEntry.Text = string.Empty;
//             IdentificationNumberEntry.Text = string.Empty;
//             PhoneEntry.Text = string.Empty;
//             EmailEntry.Text = string.Empty;
//             PasswordEntry.Text = string.Empty;
//             RolePicker.SelectedIndex = -1;
//         }
//         private static string GetStringRole(int role)
//         {
//             string strRole;
//             switch (role)
//             {
//                 case 0:
//                     strRole = "Client";
//                     break;
//                 case 1:
//                     strRole = "Operator";
//                     break;
//                 case 2:
//                     strRole = "Manager";
//                     break;
//                 case 3:
//                     strRole = "External Specialist";
//                     break;
//                 case 4:
//                     strRole = "Admin";
//                     break;
//                 default:
//                     strRole = "Default";
//                     break;
//             }
//
//             return strRole;
//         }
//     }
// }
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