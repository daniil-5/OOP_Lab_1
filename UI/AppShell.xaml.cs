using OOP_Lab_1.UI;

namespace OOP_Lab_1;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("registration", typeof(RegistrationPage));
        Routing.RegisterRoute("main", typeof(MainPage));
        Routing.RegisterRoute("login", typeof(LoginPage));
        GoToLoginPage();
    }
    private async void GoToLoginPage()
    {
        await Shell.Current.GoToAsync("login");
    }
}