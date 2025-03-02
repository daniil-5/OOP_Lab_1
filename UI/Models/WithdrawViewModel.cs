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

namespace OOP_Lab_1.UI.ViewModels
{
    public class WithdrawViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;
        private string _tableName;
        private string _userEmail;
        private double _withdrawAmount;
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

        public double WithdrawAmount
        {
            get => _withdrawAmount;
            set
            {
                if (_withdrawAmount != value)
                {
                    _withdrawAmount = value;
                    OnPropertyChanged(nameof(WithdrawAmount));
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

        public ICommand WithdrawCommand { get; }

        public WithdrawViewModel(IAccountService accountService)
        {
            _accountService = accountService;
            WithdrawCommand = new Command(async () => await WithdrawAsync());
        }

        public async Task LoadAccountsAsync()
        {
            Accounts.Clear();
            var accounts = await _accountService.GetAccountsByEmailAsync(_userEmail);
            
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

        private async Task WithdrawAsync()
        {
            if (SelectedAccount == null)
            {
                StatusMessage = "Please select an account.";
                return;
            }

            if (WithdrawAmount <= 0)
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

            if (SelectedAccount.Balance < WithdrawAmount)
            {
                StatusMessage = "Insufficient funds.";
                return;
            }

            Console.WriteLine(SelectedAccount.AccountNumber);
            bool success = await _accountService.WithdrawAsync(SelectedAccount.AccountNumber, WithdrawAmount);
            if (success)
            {
                StatusMessage = "Withdrawal successful.";
                SelectedAccount.Balance -= WithdrawAmount;
                OnPropertyChanged(nameof(SelectedAccount));
                LoadAccountsAsync();
            }
            else
            {
                StatusMessage = "Withdrawal failed. Please try again.";
            }
        }

        public void ApplyTableName(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("Table name cannot be null or empty.");
            }
            _tableName = tableName;
        }
        public void ApplyUserEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("User email cannot be null or empty.");
            }
            _userEmail = email;
        }
    }
}
