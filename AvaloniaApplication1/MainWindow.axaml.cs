using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using AvaloniaApplication1.Models;
using AvaloniaApplication1.Services;

namespace AvaloniaApplication1;

//Перечисление, которое позволит использовать одно событие для поиска
public enum SearchBy
{
    LastName,
    FirstName,
    MiddleName
}

public partial class MainWindow : Window
{
    //Список пользователей для привязки к DataGrid
    public ObservableCollection<User> Users { get; set; }
    
    //По какому критерию будет выполняться поиск
    private SearchBy _searchBy;
    
    //Объект типа ApiService для связи клиентского окна с API
    private ApiService _apiService;
    public MainWindow()
    {
        InitializeComponent();
        
        //Указываем, что в качестве привязки данных окно должно брать данные с этого класса
        DataContext = this;
    }

    //По открытию окна...
    //Обрати внимание на префикс "async", который указывает на то, что все операции в этом методе
    //не будут блокировать основной поток приложения и тем самым приложение не будет тормозить
    protected override async void OnOpened(EventArgs e)
    {
        _apiService = new ApiService();
        await RefreshDataAsync();
        //обновляем выпадающий список для фильтрации
        FilterCBox.Items = Users;
    }

    //Вспомогательный метод для обновления интерфейса
    private async Task RefreshDataAsync(IEnumerable<User> users = null)
    {
        //Сразу получаем список (слово await идёт в связке со словом async)
        //словом await помечается операция, которая не должна тормозить приложение
        //и которая должна выполниться асинхронно, т.е. в другом потоке
        //обновляем интерфейс
        users ??= await _apiService.GetUsersAsync();
        
        //Переназначем список
        Users = new ObservableCollection<User>(users);
        //Заново устанавливаем источник данных для DataGrid
        UsersDataGrid.Items = Users;
    }

    //Кнопка добавления
    private async void AddButton_Click(object? sender, RoutedEventArgs e)
    {
        var newUser = new User();
        //Передаём apiService и пустой объект User, чтобы данные нового пользователя, введённые в окне добавления
        //автоматически присвоились этому пустому объекту newUser
        var addWindow = new AddUserWindow(_apiService, newUser);
        await addWindow.ShowDialog(this);
        await RefreshDataAsync();
    }

    //Кнопка изменения
    private async void UpdButton_Click(object? sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        
        if (button?.DataContext is User user)
        {
            var addWindow = new AddUserWindow(_apiService, user);
            await addWindow.ShowDialog(this);
            var users = await _apiService.GetUsersAsync();
            await RefreshDataAsync();
        }
    }

    //Кнопка удаления
    private async void DeleteBtn_Click(object? sender, RoutedEventArgs e)
    {
        var button = sender as Button;

        if (button?.DataContext is User user)
        {
            var messageWindow = new DialogWindow(_apiService, user);
            await messageWindow.ShowDialog(this);
            var users = await _apiService.GetUsersAsync();
            await RefreshDataAsync();
        }
    }

    private async void SearchTBox_OnKeyUp(object? sender, KeyEventArgs e)
    {
        IEnumerable<User> searchUsers = null;
        
        if (sender is TextBox tBox)
        {
            searchUsers = _searchBy switch
            {
                SearchBy.FirstName => _apiService.Users.Where(u => u.FirstName.ToLower().Contains(tBox.Text.ToLower())),
                SearchBy.LastName => _apiService.Users.Where(u => u.LastName.ToLower().Contains(tBox.Text.ToLower())),
                SearchBy.MiddleName => _apiService.Users.Where(
                    u => u.MiddleName.ToLower().Contains(tBox.Text.ToLower())),
                _ => searchUsers
            };

            if (searchUsers != null)
            {
                await RefreshDataAsync();
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
        throw new NotImplementedException();
    }
}