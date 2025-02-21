using OOP_Lab_1.Core.Interfaces;
using OOP_Lab_1.Core.Services;
using OOP_Lab_1.UI;
namespace OOP_Lab_1;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }
}