using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DeskTopOdin.Services;
using System.Linq;
using Avalonia.Interactivity;
using DeskTopOdin.Models;

namespace DeskTopOdin
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<User> Users { get; set; }
        private HubService _hub;

        public bool CollectionEmpty => Users is {Count: 0};
        
        public MainWindow()
        {
            InitializeComponent();
            Users = new ObservableCollection<User>();
          DataContext = this;
        }

        protected async override void OnInitialized()
        {
            Debug.WriteLine("Initialized Component!");

            _hub = new HubService();
            Debug.WriteLine("HubService is create!");
            Users = new ObservableCollection<User>(await RefreshUsersAsync());
            Debug.WriteLine("Initialized connection!");
            Debug.WriteLine($"Get {Users.Count} users!");
        }

        private async Task<IEnumerable<User>> RefreshUsersAsync()
        {
            var users = await _hub.GetUsersAsync();

            return users;
        }

        private void InitalizeComponent()
            => AvaloniaXamlLoader.Load(this);

        private async void Button_OnClick(object? sender, RoutedEventArgs e)
        {
            Users = new ObservableCollection<User>(await RefreshUsersAsync());
        }
    }
}