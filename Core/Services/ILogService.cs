using Microsoft.Extensions.Logging.Abstractions;
using OOP_Lab_1.Core.Entities;

namespace OOP_Lab_1.Core.Services;

public interface ILogService
{
    List<LogEntry> GetLogs();
}