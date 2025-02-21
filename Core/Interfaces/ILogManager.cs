using OOP_Lab_1.Core.Entities;

namespace OOP_Lab_1.Core.Interfaces;

public interface ILogManager
{
    void LogAction(string action, User user);
    List<string> GetLogs();
}