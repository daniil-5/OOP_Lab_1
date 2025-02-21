using OOP_Lab_1.Core.Interfaces;

namespace OOP_Lab_1.Core.Entities;

public class EnterpriseAccount : Account
{
    public Enterprise Owner { get; private set; }
}