

using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Services;

namespace OOP_Lab_1.UI.Models;

public class LogsViewModel : BaseViewModel
{
    private readonly ILogService _logService;

    public List<LogEntry> Logs { get; private set; }

    public LogsViewModel(ILogService logService)
    {
        _logService = logService;
        LoadLogs();
    }

    private void LoadLogs()
    {
        try
        {
            Logs = _logService.GetLogs();
            OnPropertyChanged(nameof(Logs));
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}