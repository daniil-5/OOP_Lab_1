
using OOP_Lab_1.Core.Interfaces;
using SQLite;
namespace OOP_Lab_1.Core.Entities;

public class UserAccount : Account
    {
        public User Owner { get; private set; }
        
    }