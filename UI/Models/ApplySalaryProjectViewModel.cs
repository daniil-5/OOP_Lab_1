using System.Windows.Input;
using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;
using OOP_Lab_1.Core.Services;

namespace OOP_Lab_1.UI.Models;

public class ApplySalaryProjectViewModel : BaseViewModel
{
    private readonly ISalaryProjectService _salaryProjectService;
        private readonly IUserService _userService;

        private string _statusMessage;
        private User _currentUser;
        private Enterprise _enterprise;

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

        public ICommand ApplyCommand { get; }

        public ApplySalaryProjectViewModel(ISalaryProjectService salaryProjectService, IUserService userService, User currentUser)
        {
            _salaryProjectService = salaryProjectService;
            _userService = userService;
            _currentUser = currentUser;

            ApplyCommand = new Command(async () => await ApplyForSalaryProjectAsync());
        }

        private async Task ApplyForSalaryProjectAsync()
        {
            
            try
            {
                int workerId = await _userService.GetWorkerIdFromUserAsync(_currentUser);
                if (workerId <= 0)
                {
                    StatusMessage = "You do not work for any enterprise.";
                    return;
                }

                int isConnected = await _userService.IsConnectedToSalaryAsync(workerId);
                if (isConnected != 0)
                {
                    StatusMessage = "You alredy connected to the salary project.";
                    return;
                }
                    
                int enterpriseId = await _userService.GetEnterpriseIdByWorkerIdAsync(workerId);
                if (enterpriseId <= 0)
                    throw new ArgumentOutOfRangeException("Invalid enterprise");
                
                var enterprise = await _userService.GetEnterpriseByIdAsync(enterpriseId);
                
                if (enterprise == null)
                    throw new ArgumentOutOfRangeException("Invalid enterprise");
                
                var salaryProject = await _salaryProjectService.GetSalaryProjectByEnterpriseIdAsync(enterpriseId);

                if (salaryProject == null)
                {
                    StatusMessage = "No salary project exists for your enterprise.";
                    return;
                }
                
                if (!salaryProject.IsProcessed)
                {
                    StatusMessage = "The salary project for your enterprise is not yet processed.";
                    return;
                }

                await _userService.ConnectToSalaryAsync(workerId);
                bool isSuccess = await _salaryProjectService.IncrementConnectedWorkersAsync(salaryProject.Id);

                if (isSuccess)
                {
                    StatusMessage = "You have successfully applied for the salary project.";
                }
                else
                {
                    StatusMessage = "Failed to apply for the salary project.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"An error occurred: {ex.Message}";
            }
        }
}