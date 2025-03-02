using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Entities.Repositories;
using OOP_Lab_1.Core.Interfaces;
using OOP_Lab_1.Core.Services;
using OOP_Lab_1.Services;
using OOP_Lab_1.UI;
using OOP_Lab_1.UI.ViewModels;


namespace OOP_Lab_1
{
    public static class MauiProgram
    {
        private const string Database = "/Users/daniil_mariyn/RiderProjects/OOP_Lab_1/DataBase/appDB";
        private static string _databasePath = Path.Combine(Database);
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
            builder.Services.AddSingleton<IDatabaseService>(sp => new DatabaseService(_databasePath));
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegistrationPage>();
        
            builder.Services.AddSingleton<IBanksService>(sp => new BanksService(_databasePath));
            builder.Services.AddTransient<BanksPage>();
            
            
            builder.Services.AddSingleton<IAccountRepository>(sp => new AccountRepository(_databasePath));
            
            builder.Services.AddSingleton<IAccountService, AccountService>();
            builder.Services.AddTransient<WithdrawCashPage>();
            builder.Services.AddTransient<OpenAccountPage>();
            builder.Services.AddTransient<CloseAccountPage>();
            builder.Services.AddTransient<TransferPage>();
            
            
            
            
            builder.Services.AddSingleton<ILoanRepository>(sp => new LoanRepository(_databasePath));

            builder.Services.AddSingleton<ILoanService, LoanService>();
            builder.Services.AddTransient<GetLoanPage>();
            builder.Services.AddTransient<LoanApprovalPage>();
            
            return builder.Build();
        }
    }
    
}