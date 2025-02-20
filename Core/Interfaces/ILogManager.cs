namespace OOP_Lab_1.Core.Interfaces;

public interface ILogManager
{
    void LogAction(string action, IUser user);
    List<string> GetLogs();
}