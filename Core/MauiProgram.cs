using Microsoft.Extensions.Logging;
using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Entities.Repositories;
using OOP_Lab_1.Core.Interfaces;
using OOP_Lab_1.Core.Services;
using OOP_Lab_1.UI;
using OOP_Lab_1.UI.Models;
using OOP_Lab_1.UI.ViewModels;
using Serilog;


namespace OOP_Lab_1
{
    public static class MauiProgram
    {
        private const string Database = "/Users/daniil_mariyn/RiderProjects/OOP_Lab_1/DataBase/appDB";
        private static string _databasePath = Path.Combine(Database);
        private static string _logsPath = "/Users/daniil_mariyn/RiderProjects/OOP_Lab_1/logs/logs.txt";
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
        
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console() // Логи в консоль
                .WriteTo.File("/Users/daniil_mariyn/RiderProjects/OOP_Lab_1/logs/logs.txt")
                .CreateLogger();
            
            
            builder.Services.AddSingleton<IBankRepository>(sp => new BankRepository(_databasePath));
            builder.Services.AddSingleton<IBanksService, BanksService>();
            builder.Services.AddTransient<BanksPage>();
            
            
            builder.Services.AddSingleton<IUserRepository>(sp => new UserRepository(_databasePath));
            
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegistrationViewModel>();
            builder.Services.AddTransient<RegistrationApprovalPage>();
            builder.Services.AddTransient<RegistrationPage>();
            builder.Services.AddTransient<TransactionStatisticsPage>();
            builder.Services.AddTransient<SubmitSalaryProjectPage>();
            builder.Services.AddTransient<ApplyForSalaryPage>();
            builder.Services.AddTransient<ConnectedToSalaryPage>();
            
            
            builder.Services.AddSingleton<IAccountRepository>(sp => new AccountRepository(_databasePath));
            
            builder.Services.AddSingleton<IAccountService, AccountService>();
            builder.Services.AddTransient<WithdrawCashPage>();
            builder.Services.AddTransient<RefilAccountPage>();
            builder.Services.AddTransient<OpenAccountPage>();
            builder.Services.AddTransient<CloseAccountPage>();
            builder.Services.AddTransient<TransferPage>();
            
            
            builder.Services.AddSingleton<ILoanRepository>(sp => new LoanRepository(_databasePath));

            builder.Services.AddSingleton<ILoanService, LoanService>();
            builder.Services.AddTransient<GetLoanPage>();
            builder.Services.AddTransient<LoanApprovalPage>();
            
            builder.Services.AddSingleton<ISalaryProjectRepository>(sp => new SalaryProjectRepository(_databasePath));

            builder.Services.AddSingleton<ISalaryProjectService, SalaryProjectService>();
            builder.Services.AddTransient<SubmitSalaryProjectPage>();
            builder.Services.AddTransient<SalaryApprovalPage>();
            builder.Services.AddTransient<ApplyForSalaryPage>();
            builder.Services.AddTransient<ConnectedToSalaryPage>();
            
            builder.Services.AddSingleton<ILogService>(sp => new LogService(_logsPath));
            builder.Services.AddTransient<LogsPage>();
            
            
            
            return builder.Build();
        }
    }
    
}