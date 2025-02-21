namespace OOP_Lab_1.Core.Interfaces;

public interface IAdminActions
{
    List<string> ViewLogs();
    void CancelUserAction(string actionId);
}