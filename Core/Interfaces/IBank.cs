namespace OOP_Lab_1.Core.Interfaces;

public interface IBank
{
    string Name { get; set; }
    List<IUser> Users { get; set; }
    List<IEnterprise> Companies { get; set; }
    List<IAccount> Accounts { get; set; }

    void AddUser(IUser user);
    void RemoveUser(IUser user);
    IAccount OpenAccount(IUser user);
}