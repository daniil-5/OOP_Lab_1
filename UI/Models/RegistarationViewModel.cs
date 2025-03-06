using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;
using System.Windows.Input;


namespace OOP_Lab_1.UI.Models
{
    public class RegistrationViewModel : BaseViewModel
    {
        private readonly IUserService _userService;

        private string _fullName;
        private string _passportNumber;
        private string _identificationNumber;
        private string _phone;
        private string _email;
        private string _password;
        private int _selectedRoleIndex = -1;
        private string _statusMessage;

        public string FullName
        {
            get => _fullName;
            set
            {
                if (_fullName != value)
                {
                    _fullName = value;
                    OnPropertyChanged(nameof(FullName));
                }
            }
        }

        public string PassportNumber
        {
            get => _passportNumber;
            set
            {
                if (_passportNumber != value)
                {
                    _passportNumber = value;
                    OnPropertyChanged(nameof(PassportNumber));
                }
            }
        }

        public string IdentificationNumber
        {
            get => _identificationNumber;
            set
            {
                if (_identificationNumber != value)
                {
                    _identificationNumber = value;
                    OnPropertyChanged(nameof(IdentificationNumber));
                }
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                if (_phone != value)
                {
                    _phone = value;
                    OnPropertyChanged(nameof(Phone));
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public int SelectedRoleIndex
        {
            get => _selectedRoleIndex;
            set
            {
                if (_selectedRoleIndex != value)
                {
                    _selectedRoleIndex = value;
                    OnPropertyChanged(nameof(SelectedRoleIndex));
                }
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (_statusMessage != value)
                {
                    _statusMessage = value;
                    OnPropertyChanged(nameof(StatusMessage));
                }
            }
        }

        public string BankId { get; set; } = "Unknown";

        public ICommand RegisterCommand { get; }

        public RegistrationViewModel(IUserService userService)
        {
            _userService = userService;
            RegisterCommand = new Command(async () => await RegisterAsync());
        }

        private async Task RegisterAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(FullName) ||
                    string.IsNullOrEmpty(PassportNumber) ||
                    string.IsNullOrEmpty(IdentificationNumber) ||
                    string.IsNullOrEmpty(Phone) ||
                    string.IsNullOrEmpty(Email) ||
                    string.IsNullOrEmpty(Password) ||
                    SelectedRoleIndex == -1)
                {
                    StatusMessage = "Please fill in all fields.";
                    return;
                }

                if (!IsValidEmail(Email))
                {
                    StatusMessage = "Please enter a valid email address.";
                    return;
                }

                if (Password.Length < 6)
                {
                    StatusMessage = "Password must be at least 6 characters long.";
                    return;
                }

                bool userExists = await _userService.UserExistsByEmailAsync(Email, BankId);
                if (userExists)
                {
                    StatusMessage = "A user with this email already exists.";
                    return;
                }

                var newUser = new User
                {
                    FullName = FullName,
                    PassportNumber = PassportNumber,
                    IdentificationNumber = IdentificationNumber,
                    Phone = Phone,
                    Email = Email,
                    Password = Password, // Note: Hash the password before saving in a real app
                    Role = SelectedRoleIndex
                };

                bool isAdded = await _userService.RegisterUserAsync(newUser, BankId);
                if (isAdded)
                {
                    StatusMessage = $"User {FullName} registered as {GetStringRole(SelectedRoleIndex)}.";
                    await Shell.Current.DisplayAlert("Registration successful", $"User {FullName} registered as {GetStringRole(SelectedRoleIndex)}.", "OK");
                    ClearForm();
                    // await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Registration failed", "Failed to register the user. Please try again", "OK");
                    StatusMessage = "Failed to register the user. Please try again.";
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Registration failed", "Failed to register the user. Please try again", "OK");
                StatusMessage = $"An error occurred: {ex.Message}";
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void ClearForm()
        {
            FullName = string.Empty;
            PassportNumber = string.Empty;
            IdentificationNumber = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            SelectedRoleIndex = -1;
        }

        private static string GetStringRole(int role)
        {
            return role switch
            {
                0 => "Client",
                1 => "Operator",
                2 => "Manager",
                3 => "External Specialist",
                4 => "Admin",
                _ => "Default"
            };
        }
    }
}