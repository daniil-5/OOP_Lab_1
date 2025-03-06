using System.Collections.ObjectModel;
using System.Windows.Input;
using OOP_Lab_1.Core.Entities;
using OOP_Lab_1.Core.Interfaces;

namespace OOP_Lab_1.UI.Models;

public class UserApprovalViewModel : BaseViewModel
{
    private readonly IUserService _userService;
    private string _bic;

    public ObservableCollection<User> Users { get; set; }
        
    public ICommand ApproveCommand { get; }
    public ICommand RejectCommand { get; }

    public UserApprovalViewModel(IUserService userService)
    {
        _userService = userService;
        Users = new ObservableCollection<User>();

        ApproveCommand = new Command<User>(async (User) => await ApproveUser(User));
        RejectCommand = new Command<User>(async (User) => await RejectUser(User));
    }
        
    public void ApplyBic(string bic)
    {
        _bic = bic;
        LoadUsers();
    }

    // Load Pending Users from the Service
    private async void LoadUsers()
    {
        Console.WriteLine("Loading Users");
        var pendingUsers = await _userService.GetPendingUsersAsync(_bic);
        Users.Clear();
        foreach (var User in pendingUsers)
        {
            Users.Add(User);
        }
    }

    // Approve User
    private async Task ApproveUser(User user)
    {
        await _userService.ApproveUserStatusAsync(user, _bic);
        Users.Remove(user);
    }

    // Reject User
    private async Task RejectUser(User user)
    {
        await _userService.DeleteUserAsync(user.Id);
        Users.Remove(user);
    }
}