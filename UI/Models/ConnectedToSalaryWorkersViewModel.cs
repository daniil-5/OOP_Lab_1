using System.Collections.ObjectModel;
using System.Windows.Input;
using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;
using OOP_Lab_1.Core.Services;

namespace OOP_Lab_1.UI.Models;

public class ConnectedToSalaryWorkersViewModel : BaseViewModel
{
    private readonly IUserService _userService;
    private readonly ISalaryProjectService _salaryService;

    public ObservableCollection<Worker> Workers { get; set; }
    
    public ICommand DisconnectCommand { get; }

    public ConnectedToSalaryWorkersViewModel(IUserService userService, ISalaryProjectService salaryService)
    {
        _userService = userService;
        _salaryService = salaryService;
        Workers = new ObservableCollection<Worker>();
        DisconnectCommand = new Command<Worker>(async (worker) => await DisconnectTransaction(worker));
    }
    
    public async void LoadTransactions()
    {
        var workers = await _userService.GetConnectedToSalaryWorkersAsync();
        Workers.Clear();
        foreach (var worker in workers)
        {
            Workers.Add(worker);
        }
    }
    
    private async Task DisconnectTransaction(Worker worker)
    {
        await _userService.DisconnectFromSalaryAsync(worker.Id);
        int enterpriseId = await _userService.GetEnterpriseIdByWorkerIdAsync(worker.Id);
        if (enterpriseId <= 0)
            throw new ArgumentOutOfRangeException("Invalid enterprise id provided");
        Console.WriteLine("3");
        var salaryProject = await _salaryService.GetSalaryProjectByEnterpriseIdAsync(enterpriseId);
        Console.WriteLine("4");
        if (salaryProject == null)
            throw new ArgumentOutOfRangeException("Invalid enterprise");
        Console.WriteLine("5");
        await _salaryService.DecrementConnectedWorkersAsync(salaryProject.Id);
        Workers.Remove(worker);
    }
}