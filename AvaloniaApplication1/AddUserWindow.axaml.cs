using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaApplication1.Models;
using AvaloniaApplication1.Services;

namespace AvaloniaApplication1;

public partial class AddUserWindow : Window
{
    private enum WindowStates
    {
        Add,
        Update
    }
    //Объект, который будет хранить данные для добавления
    public User NewUser { get; set; }
    private ApiService ApiService { get; set; }
    private WindowStates _currentState;

    public AddUserWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    public AddUserWindow(ApiService api, User newUser = null) :this()
    {
        ApiService = api;

        if (newUser == null)
        {
            NewUser = new User();
            _currentState = WindowStates.Add;
        }
        else
        {
            NewUser = newUser;
            _currentState = WindowStates.Update;
        }


        DataContext = NewUser;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private async void AddBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        switch (_currentState)
        {
            case WindowStates.Add:
                await ApiService.AddUserAsync(NewUser);
                break;
            case WindowStates.Update:
                await ApiService.UpdateUserAsync(NewUser);
                break;
        }
        
        Close();
    }
}