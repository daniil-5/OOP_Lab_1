using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;
using OOP_Lab_1.UI.Models;

namespace OOP_Lab_1.UI.Models
{
    public class RefilAccountViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;
        private string _userEmail;
        private string _bankId;
        private double _refillAmount;
        private string _statusMessage;
        private UserAccount _selectedAccount;

        public ObservableCollection<UserAccount> Accounts { get; private set; } = new ObservableCollection<UserAccount>();

        public UserAccount SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                if (_selectedAccount != value)
                {
                    _selectedAccount = value;
                    OnPropertyChanged(nameof(SelectedAccount));
                }
            }
        }

        public double RefillAmount
        {
            get => _refillAmount;
            set
            {
                if (_refillAmount != value)
                {
                    _refillAmount = value;
                    OnPropertyChanged(nameof(RefillAmount));
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

        public ICommand RefillCommand { get; }

        public RefilAccountViewModel(IAccountService accountService)
        {
            _accountService = accountService;
            RefillCommand = new Command(async () => await RefillAsync());
        }

        public async Task LoadAccountsAsync()
        {
            Accounts.Clear();
            var accounts = await _accountService.GetAccountsByEmailAsync(_userEmail, _bankId);
            
            if (accounts == null || !accounts.Any())
            {
                StatusMessage = "No accounts found.";
                return;
            }
            foreach (var account in accounts)
            {
                Accounts.Add(account);
            }
        }

        private async Task RefillAsync()
        {
            if (SelectedAccount == null)
            {
                StatusMessage = "Please select an account.";
                return;
            }

            if (RefillAmount <= 0)
            {
                StatusMessage = "Amount must be greater than zero.";
                return;
            }

            if (SelectedAccount.IsBlocked)
            {
                StatusMessage = "This account is blocked.";
                return;
            }

            if (SelectedAccount.IsFrozen)
            {
                StatusMessage = "This account is frozen.";
                return;
            }

            Console.WriteLine(SelectedAccount.AccountNumber);
            bool success = await _accountService.RefillAsync(SelectedAccount.AccountNumber, RefillAmount);
            if (success)
            {
                StatusMessage = "Refilling successful.";
                SelectedAccount.Balance += RefillAmount;
                OnPropertyChanged(nameof(SelectedAccount));
                LoadAccountsAsync();
            }
            else
            {
                StatusMessage = "Refilling failed. Please try again.";
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

        public void ApplyBIC(string bankId)
        {
            if (string.IsNullOrEmpty(bankId))
            {
                throw new ArgumentException("BankId cannot be null or empty.");
            }
            _bankId = bankId;
        }
    }
}
