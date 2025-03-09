using System.Collections.ObjectModel;
using System.Windows.Input;
using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;
using OOP_Lab_1.Core.Services;
using OOP_Lab_1.UI.Models;

namespace OOP_Lab_1.UI.Models
{
    public class SalaryApprovalViewModel : BaseViewModel
    {
        private readonly ISalaryProjectService _salaryProjectService;
        public ObservableCollection<SalaryProject> SalaryProjects { get; set; }

        public ICommand ApproveCommand { get; }
        public ICommand RejectCommand { get; }

        public SalaryApprovalViewModel(ISalaryProjectService salaryProjectService)
        {
            _salaryProjectService = salaryProjectService;

            SalaryProjects = new ObservableCollection<SalaryProject>();

            ApproveCommand = new Command<SalaryProject>(async (project) => await ApproveSalaryProjectAsync(project));
            RejectCommand = new Command<SalaryProject>(async (project) => await RejectSalaryProjectAsync(project));

            LoadPendingSalaryProjects();
        }

        private async void LoadPendingSalaryProjects()
        {
            try
            {
                var pendingProjects = await _salaryProjectService.GetPendingSalaryProjectsAsync();
                
                SalaryProjects.Clear();
                foreach (var project in pendingProjects)
                {
                    SalaryProjects.Add(project);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to load salary projects: {ex.Message}", "OK");
            }
        }

        private async Task ApproveSalaryProjectAsync(SalaryProject project)
        {
            try
            {
                bool isSuccess = await _salaryProjectService.ProcessSalaryProjectAsync(project.Id);

                if (isSuccess)
                {
                    SalaryProjects.Remove(project);
                    await Shell.Current.DisplayAlert("Success", "Salary project approved successfully.", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to approve salary project.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async Task RejectSalaryProjectAsync(SalaryProject project)
        {
            try
            {
                bool isSuccess = await _salaryProjectService.RejectSalaryProjectAsync(project.Id);

                if (isSuccess)
                {
                    SalaryProjects.Remove(project);
                    await Shell.Current.DisplayAlert("Success", "Salary project rejected successfully.", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Failed to reject salary project.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}