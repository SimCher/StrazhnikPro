using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using AvaloniaApplication1.Models;
using AvaloniaApplication1.Services;

namespace AvaloniaApplication1;

public enum SearchBy
{
    LastName,
    FirstName,
    MiddleName
}

public partial class MainWindow : Window
{
    public string WindowTitle { get; } = "СтражникПРО";

    public ObservableCollection<User> Users { get; set; }

    private SearchBy _searchBy;
    
    

    private ApiService _apiService;
    public MainWindow()
    {
        InitializeComponent();

        DataContext = this;
    }

    protected override async void OnOpened(EventArgs e)
    {
        _apiService = new ApiService();
        var users = await _apiService.GetUsersAsync();
        RefreshData(users);
        FilterCBox.Items = Users;
    }

    private void RefreshData(IEnumerable<User> usersCollection)
    {
        Users = new ObservableCollection<User>(usersCollection);
        UsersDataGrid.Items = Users;
    }

    private async void AddButton_Click(object? sender, RoutedEventArgs e)
    {
        var newUser = new User();
        var addWindow = new AddUserWindow(_apiService, newUser);
        await addWindow.ShowDialog(this);
        var users = await _apiService.GetUsersAsync();
        RefreshData(users);
    }

    private async void UpdButton_Click(object? sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        
        if (button?.DataContext is User user)
        {
            var addWindow = new AddUserWindow(_apiService, user);
            await addWindow.ShowDialog(this);
            var users = await _apiService.GetUsersAsync();
            RefreshData(users);
        }
    }

    private async void DeleteBtn_Click(object? sender, RoutedEventArgs e)
    {
        var button = sender as Button;

        if (button?.DataContext is User user)
        {
            var messageWindow = new DialogWindow(_apiService, user);
            await messageWindow.ShowDialog(this);
            var users = await _apiService.GetUsersAsync();
            RefreshData(users);
        }
    }

    private void SearchTBox_OnKeyUp(object? sender, KeyEventArgs e)
    {
        IEnumerable<User> searchUsers = null;
        
        if (sender is TextBox tBox)
        {
            // switch (_searchBy)
            // {
            //     case SearchBy.FirstName:
            //         searchUsers = _apiService.GetUsersWithFirstNameLike(tBox.Text);
            //         break;
            //     case SearchBy.LastName:
            //         searchUsers = _apiService.GetUsersWithLastNameLike(tBox.Text);
            //         break;
            //     case SearchBy.MiddleName:
            //         searchUsers = _apiService.GetUsersWithMiddleNameLike(tBox.Text);
            //         break;
            // }

            if (searchUsers != null)
            {
                RefreshData(searchUsers);
            }
        }
    }

    private void SearchByClick(object? sender, RoutedEventArgs e)
    {
        if ((bool)ByLNameRBtn.IsChecked)
        {
            _searchBy = SearchBy.LastName;
        }
        else if ((bool) ByFNameRBtn.IsChecked)
        {
            _searchBy = SearchBy.FirstName;
        }
        else if ((bool) ByMNameRBtn.IsChecked)
        {
            _searchBy = SearchBy.MiddleName;
        }
        else
        {
            ByLNameRBtn.IsChecked = true;
            _searchBy = SearchBy.LastName;
        }
    }

    private void FilterCBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        // var usersWithLastNameLike = _apiService.GetUsersWithLastNameLike(FilterCBox.SelectedItem?.ToString());
        // RefreshData(usersWithLastNameLike);
    }

    private void UsersDataGrid_OnCellPointerPressed(object? sender, DataGridCellPointerPressedEventArgs e)
    {
        var dGrid = sender as DataGrid;
        var item = dGrid?.SelectedItem as User;
        ClickedUserIdTBlock.Text = item?.Id.ToString();
    }
}