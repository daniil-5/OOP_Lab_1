﻿using OOP_Lab_1.UI;

namespace OOP_Lab_1;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("registration", typeof(RegistrationPage));
        Routing.RegisterRoute("main", typeof(MainPage));
        Routing.RegisterRoute("login", typeof(LoginPage));
        Routing.RegisterRoute("banks", typeof(BanksPage));
        Routing.RegisterRoute("openAccount", typeof(OpenAccountPage));
        Routing.RegisterRoute("closeAccount", typeof(CloseAccountPage));
        Routing.RegisterRoute("getLoan", typeof(GetLoanPage));
        Routing.RegisterRoute("LoanApprovement", typeof(LoanApprovalPage));
        Routing.RegisterRoute("withdrawCash", typeof(WithdrawCashPage));
        Routing.RegisterRoute("transfer", typeof(TransferPage));
        Routing.RegisterRoute("refillBalance", typeof(RefilAccountPage));
        Routing.RegisterRoute("UserApprovement", typeof(RegistrationApprovalPage));
        Routing.RegisterRoute("transactionStat", typeof(TransactionStatisticsPage));
        Routing.RegisterRoute("submitSalary", typeof(SubmitSalaryProjectPage));
        Routing.RegisterRoute("approveSalary", typeof(SalaryApprovalPage));
        Routing.RegisterRoute("applyForSalary", typeof(ApplyForSalaryPage));
        Routing.RegisterRoute("connectedToSalary", typeof(ConnectedToSalaryPage));
        Routing.RegisterRoute("showLogs", typeof(LogsPage));
        GoToLoginPage();
    }
    private async void GoToLoginPage()
    {
        await Shell.Current.GoToAsync("///login");
    }
}