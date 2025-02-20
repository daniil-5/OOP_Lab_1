using SQLite;
namespace OOP_Lab_1.Core.Entities;

public abstract class Bank
{
    public string Name { get; set; }
    public string BIC { get; set; }
    public List<Client> Clients { get; private set; } = new List<Client>();
    public List<Enterprise> Enterprises { get; private set; } = new List<Enterprise>();
    public List<Account> Accounts { get; private set; } = new List<Account>();

    public void RegisterClient(Client client)
    {
        Clients.Add(client);
    }

    public void RegisterEnterprise(Enterprise enterprise)
    {
        Enterprises.Add(enterprise);
    }
}