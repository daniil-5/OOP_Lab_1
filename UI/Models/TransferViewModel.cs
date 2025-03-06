using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;

namespace OOP_Lab_1.UI.Models
{
    public class TransferViewModel : INotifyPropertyChanged
    {
        private readonly IAccountService _accountService;
        private UserAccount _selectedFromAccount;
        private UserAccount _selectedToAccount;
        private double _transferAmount;
        private string _statusMessage;
        private Color _statusColor;
        private string _userEmail;
        private string _bankId;

        public ObservableCollection<UserAccount> MyAccounts { get; private set; } = new ObservableCollection<UserAccount>();
        public ObservableCollection<UserAccount> Accounts { get; private set; } = new ObservableCollection<UserAccount>();
        
        public UserAccount SelectedFromAccount
        {
            get => _selectedFromAccount;
            set
            {
                if (_selectedFromAccount != value)
                {
                    _selectedFromAccount = value;
                    OnPropertyChanged();
                }
            }
        }

        public UserAccount SelectedToAccount
        {
            get => _selectedToAccount;
            set
            {
                if (_selectedToAccount != value)
                {
                    _selectedToAccount = value;
                    OnPropertyChanged();
                }
            }
        }

        public double TransferAmount
        {
            get => _transferAmount;
            set
            {
                if (_transferAmount != value)
                {
                    _transferAmount = value;
                    OnPropertyChanged();
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
                    OnPropertyChanged();
                }
            }
        }

        public Color StatusColor
        {
            get => _statusColor;
            set
            {
                if (_statusColor != value)
                {
                    _statusColor = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand TransferCommand { get; }

        public TransferViewModel(IAccountService accountService)
        {
            _accountService = accountService;
            TransferCommand = new Command(async () => await TransferAsync());
            // LoadAccountsAsync().Wait();
        }

        public async Task LoadAccountsAsync()
        {
            
            try
            {
                var accounts = await _accountService.GetAllAccountsAsync();
                Accounts.Clear();
                if (accounts != null && accounts.Any())
                {
                    foreach (var account in accounts)
                    {
                        Accounts.Add(account);
                    }
                }
                else
                {
                    // await DisplayAlert("No Accounts", "You have no accounts to close.", "OK");
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex)
            {
                // await DisplayAlert("Error", $"Failed to load accounts: {ex.Message}", "OK");
            }
            
            var myaccounts = await _accountService.GetAccountsByEmailAsync(_userEmail, _bankId);
            MyAccounts.Clear();
            foreach (var acc in myaccounts)
            {
                MyAccounts.Add(acc);
            }
            
        }

        private async Task TransferAsync()
        {
            if (SelectedFromAccount == null || SelectedToAccount == null)
            {
                StatusMessage = "Please select both source and destination accounts.";
                StatusColor = Colors.Red;
                return;
            }

            if (TransferAmount <= 0)
            {
                StatusMessage = "Amount must be greater than zero.";
                StatusColor = Colors.Red;
                return;
            }

            bool success = await _accountService.TransferAsync(SelectedFromAccount.AccountNumber.ToString(), SelectedToAccount.AccountNumber.ToString(), TransferAmount);
            if (success)
            {
                StatusMessage = "Transfer successful!";
                StatusColor = Colors.Green;
            }
            else
            {
                StatusMessage = "Transfer failed. Please try again.";
                StatusColor = Colors.Red;
            }
        }
        public void ApplyUserEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("User email cannot be null or empty.");
            }
            _userEmail = email;
        }
        
        public void ApplyBic(string bankId)
        {
            if (string.IsNullOrEmpty(bankId))
            {
                throw new ArgumentException("BankId cannot be null or empty.");
            }
            _bankId = bankId;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}