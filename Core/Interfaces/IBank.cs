using OOP_Lab_1.Core.Entities;

namespace OOP_Lab_1.Core.Interfaces;

public interface IBank
{
    string Name { get; set; }
    string BIC { get; set; }
    List<User> Users { get; set; }
    List<IEnterprise> Companies { get; set; }
    List<IAccount> Accounts { get; set; }

    void AddUser(User user);
    void RemoveUser(User user);
    IAccount OpenAccount(User user);
}