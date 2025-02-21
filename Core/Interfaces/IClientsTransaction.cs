using OOP_Lab_1.Core.Entities;

namespace OOP_Lab_1.Core.Interfaces;

public class IClientsTransaction
{
    User FromClient { get; }
    User ToClient { get; }
    double Ammount { get; }
    string Status { get; set; }
}