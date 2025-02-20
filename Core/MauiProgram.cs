﻿using OOP_Lab_1.Core.Interfaces;
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
        return builder.Build();
    }
}