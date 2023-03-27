using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using AvaloniaApplication1.Models;
using AvaloniaApplication1.Services;

namespace AvaloniaApplication1;

public partial class DialogWindow : Window
{
    public string Question { get; set; }
    private ApiService ApiService { get; set; }
    
    private User UserToDelete { get; set; }
    public DialogWindow()
    {
        InitializeComponent();

        DataContext = this;
    }

    public DialogWindow(ApiService api, User user) : this()
    {
        ApiService = api;
        UserToDelete = user;
        Question = $"Вы действительно хотите удалить пользователя: {UserToDelete.LastName} " +
                   $"{UserToDelete.FirstName} " +
                   $"{UserToDelete.MiddleName}?)";
        
        InitializeComponent();

        DataContext = this;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private async void Yes_OnClick(object? sender, RoutedEventArgs e)
    {
        await ApiService.DeleteUserAsync(UserToDelete.Id);
        Close();
    }

    private void No_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}