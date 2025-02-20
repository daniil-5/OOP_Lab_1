using OOP_Lab_1.Services;
namespace OOP_Lab_1.Core.Services;

public static class DatabaseServiceFactory
{
    public static DatabaseService CreateDatabaseService()
    {
        string databasePath = Path.Combine("/Users/daniil_mariyn/RiderProjects/OOP_Lab_1/DataBase/appDB");
        return new DatabaseService(databasePath);
    }
}