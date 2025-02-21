using OOP_Lab_1.Core.Interfaces;
using OOP_Lab_1.Core.Services;
using OOP_Lab_1.Services;
using OOP_Lab_1.UI;

namespace OOP_Lab_1;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        builder.Services.AddSingleton<IDatabaseService>(sp =>
        {
            return DatabaseServiceFactory.CreateDatabaseService();
        });
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegistrationPage>();
        
        builder.Services.AddSingleton<IBanksService>(sp =>
        {
            string databasePath = Path.Combine("/Users/daniil_mariyn/RiderProjects/OOP_Lab_1/DataBase/appDB");
            return new BanksService(databasePath);
        });
        builder.Services.AddTransient<BanksPage>();
        
        return builder.Build();
    }
}