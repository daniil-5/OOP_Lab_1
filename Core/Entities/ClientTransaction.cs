using OOP_Lab_1.Core.Interfaces;

namespace OOP_Lab_1.Core.Entities;

public class ClientTransaction : IClientsTransaction
{
    private User FromClient { get; set; }
    private User ToClient { get; set; }
    private double Ammount { get; set; }
    private string Status { get; set; }
}