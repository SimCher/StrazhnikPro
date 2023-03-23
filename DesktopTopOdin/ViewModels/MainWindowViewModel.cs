using System.Collections.ObjectModel;
using DesktopTopOdin.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ReactiveUI;

namespace DesktopTopOdin.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ObservableCollection<User> _users;

    public ObservableCollection<User> Users
    {
        get => _users;
        private set => this.RaiseAndSetIfChanged(ref _users, value);
    }

    public MainWindowViewModel()
    {
        
    }

    protected override void Initialize()
    {
        
    }
}