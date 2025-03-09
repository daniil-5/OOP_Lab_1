using System.Windows.Input;
using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;
using OOP_Lab_1.Core.Services;

namespace OOP_Lab_1.UI.Models;

public class SalaryProjectViewModel : BaseViewModel
{
    private readonly ISalaryProjectService _salaryProjectService;
    private readonly IUserService _userService;
    private Enterprise _enterprise;
        private string _legalName;
        private string _type;
        private string _unp;
        private string _legalAddress;
        private int _period;
        private string _statusMessage;
        private User _user;

        public void ApplyUser(User user)
        {
            _user = user;
            LoadEnterpriseDetails();
        }

        public string LegalName
        {
            get => _legalName;
            set
            {
                if (_legalName != value)
                {
                    _legalName = value;
                    OnPropertyChanged(nameof(LegalName));
                }
            }
        }

        public string Type
        {
            get => _type;
            set
            {
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged(nameof(Type));
                }
            }
        }

        public string UNP
        {
            get => _unp;
            set
            {
                if (_unp != value)
                {
                    _unp = value;
                    OnPropertyChanged(nameof(UNP));
                }
            }
        }

        public string LegalAddress
        {
            get => _legalAddress;
            set
            {
                if (_legalAddress != value)
                {
                    _legalAddress = value;
                    OnPropertyChanged(nameof(LegalAddress));
                }
            }
        }

        public int Period
        {
            get => _period;
            set
            {
                if (_period != value)
                {
                    _period = value;
                    OnPropertyChanged(nameof(Period));
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

        public ICommand SubmitCommand { get; }

        public SalaryProjectViewModel(ISalaryProjectService salaryProjectService, IUserService userService)
        {
            _salaryProjectService = salaryProjectService;
            _userService = userService;
            SubmitCommand = new Command(async () => await SubmitSalaryProjectAsync());
        }

        private async void LoadEnterpriseDetails()
        {
            try
            {
                int currentUserId = _user.Id;
                int enterpriseId = await _userService.GetEnterpriseIdByUserIdAsync(currentUserId);
                

                if (enterpriseId > 0)
                {
                    var enterprise = await _userService.GetEnterpriseByIdAsync(enterpriseId);
                    _enterprise = enterprise;
                    if (enterprise != null)
                    {
                        LegalName = enterprise.LegalName;
                        Type = enterprise.Type;
                        UNP = enterprise.UNP;
                        LegalAddress = enterprise.LegalAddress;
                    }
                    else
                    {
                        StatusMessage = "Enterprise details not found.";
                    }
                }
                else
                {
                    StatusMessage = "No enterprise associated with the current user.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"An error occurred: {ex.Message}";
            }
        }

        private async Task SubmitSalaryProjectAsync()
        {
            try
            {
                if (Period <= 0)
                {
                    StatusMessage = "Please enter a valid period.";
                    return;
                }
                
                int currentUserId = _user.Id;
                
                int enterpriseId = await _userService.GetEnterpriseIdByUserIdAsync(currentUserId);

                if (enterpriseId <= 0)
                {
                    StatusMessage = "No enterprise associated with the current user.";
                    return;
                }

                var salaryProject = new SalaryProject
                {
                    IsProcessed = false,
                    EnterpriseId = enterpriseId,
                    ApprovedDate = DateTime.Now,
                    Period = Period
                };
                
                bool isSuccess = await _salaryProjectService.SaveSalaryProjectAsync(salaryProject, _enterprise.LegalName);
                if (isSuccess)
                {
                    StatusMessage = "Salary Project submitted successfully.";
                    await Shell.Current.DisplayAlert("Success", "Salary Project submitted successfully.", "OK");
                    ClearForm();
                }
                else
                {
                    StatusMessage = "Failed to submit Salary Project.";
                    await Shell.Current.DisplayAlert("Error", "Failed to submit Salary Project.", "OK");
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"An error occurred: {ex.Message}";
                Console.WriteLine("11`1111");
                await Shell.Current.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private void ClearForm()
        {
            Period = 0;
        }
}