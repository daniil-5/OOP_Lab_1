using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OOP_Lab_1.Services;
using System.Net.Mail;
using System.Threading.Tasks;
using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;
using OOP_Lab_1.Core.Services;

namespace OOP_Lab_1.UI
{
    public partial class RegistrationPage : ContentPage
    {
        private readonly IDatabaseService _databaseService;

        public RegistrationPage(IDatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;
        }
        // public RegistrationPage()
        // {
        //     InitializeComponent();
        //     _databaseService = DatabaseServiceFactory.CreateDatabaseService();
        // }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            string fullName = FullNameEntry.Text?.Trim() ?? string.Empty;
            string passportNumber = PassportNumberEntry.Text?.Trim() ?? string.Empty;
            string identificationNumber = IdentificationNumberEntry.Text?.Trim() ?? string.Empty;
            string phone = PhoneEntry.Text?.Trim() ?? string.Empty;
            string email = EmailEntry.Text?.Trim() ?? string.Empty;
            string password = PasswordEntry.Text?.Trim() ?? string.Empty;
            int selectedRole = RolePicker.SelectedIndex;
            // Validate inputs
            if (string.IsNullOrEmpty(fullName) || 
                string.IsNullOrEmpty(passportNumber) || 
                string.IsNullOrEmpty(identificationNumber) || 
                string.IsNullOrEmpty(phone) || 
                string.IsNullOrEmpty(email) || 
                string.IsNullOrEmpty(password) ||
                selectedRole == -1)
            {
                await DisplayAlert("Validation Error", "Please fill in all fields.", "OK");
                return;
            }

            if (!IsValidEmail(email))
            {
                await DisplayAlert("Validation Error", "Please enter a valid email address.", "OK");
                return;
            }

            if (password.Length < 6)
            {
                await DisplayAlert("Validation Error", "Password must be at least 6 characters long.", "OK");
                return;
            }
            
            bool userExists = await _databaseService.UserExistsByEmailAsync(email);
            if (userExists)
            {
                await DisplayAlert("Error", "A user with this email already exists.", "OK");
                return;
            }
            
            var newUser = new User
            {
                FullName = fullName,
                PassportNumber = passportNumber,
                IdentificationNumber = identificationNumber,
                Phone = phone,
                Email = email,
                Password = password, // Note: Hash the password before saving in a real app
                Role = selectedRole
            };

            bool isAdded = await _databaseService.AddUserAsync(newUser);
            if (isAdded)
            {
                await DisplayAlert("Success", $"User {fullName} registered as {selectedRole}.", "OK");
                ClearForm();
            }
            else
            {
                await DisplayAlert("Error", "Failed to register the user. Please try again.", "OK");
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void ClearForm()
        {
            FullNameEntry.Text = string.Empty;
            PassportNumberEntry.Text = string.Empty;
            IdentificationNumberEntry.Text = string.Empty;
            PhoneEntry.Text = string.Empty;
            EmailEntry.Text = string.Empty;
            PasswordEntry.Text = string.Empty;
            RolePicker.SelectedIndex = -1;
        }
    }
}
