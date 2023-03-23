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
    
    

    private UserService _userService;
    public MainWindow()
    {
        InitializeComponent();

        DataContext = this;
    }

    protected override async void OnOpened(EventArgs e)
    {
        _userService = new UserService();
        await _userService.Initialize();
        RefreshData(_userService.Users);
        FilterCBox.Items = Users;
    }

    private void RefreshData(IEnumerable<User> usersCollection)
    {
        Users = new ObservableCollection<User>(usersCollection);
        UsersDataGrid.Items = Users;
    }

    private void SearchTBox_OnKeyUp(object? sender, KeyEventArgs e)
    {
        IEnumerable<User> searchUsers = null;
        
        if (sender is TextBox tBox)
        {
            switch (_searchBy)
            {
                case SearchBy.FirstName:
                    searchUsers = _userService.GetUsersWithFirstNameLike(tBox.Text);
                    break;
                case SearchBy.LastName:
                    searchUsers = _userService.GetUsersWithLastNameLike(tBox.Text);
                    break;
                case SearchBy.MiddleName:
                    searchUsers = _userService.GetUsersWithMiddleNameLike(tBox.Text);
                    break;
            }

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
        var usersWithLastNameLike = _userService.GetUsersWithLastNameLike(FilterCBox.SelectedItem?.ToString());
        RefreshData(usersWithLastNameLike);
    }
}