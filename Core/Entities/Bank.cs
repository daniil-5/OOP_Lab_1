using OOP_Lab_1.Core.Interfaces;
using SQLite;
namespace OOP_Lab_1.Core.Entities;

public abstract class Bank
{
    public string Name { get; set; }
    public string BIC { get; set; }
    public List<Client> Clients { get; private set; } = new List<Client>();
    public List<IAccount> Accounts { get; private set; } = new List<IAccount>();

    public void RegisterClient(Client client)
    {
        Clients.Add(client);
    }
    
}