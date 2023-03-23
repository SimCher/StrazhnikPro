using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvaloniaApplication1.Models;

namespace AvaloniaApplication1.Services;

//Класс, предоставляющий упрощённые операции для работы
//с пользователями
public class UserService
{
    private ApiService _api;

    public List<User> Users { get; private set; }

    public UserService()
    {
        _api = new ApiService();
    }

    public async Task Initialize()
    {
        var users = await _api.GetUsersAsync();

        Users = users.ToList();
    }

    public IEnumerable<User> GetUsersWithLastNameLike(string query)
    {
        if (string.IsNullOrEmpty(query)) return Users;
        var lowerQuery = query.ToLower();

        return Users.Where(u => u.LastName.ToLower().Contains(lowerQuery));
    }
    
    public IEnumerable<User> GetUsersWithFirstNameLike(string query)
    {
        if (string.IsNullOrEmpty(query)) return Users;
        var lowerQuery = query.ToLower();

        return Users.Where(u => u.FirstName.ToLower().Contains(lowerQuery));
    }
    
    public IEnumerable<User> GetUsersWithMiddleNameLike(string query)
    {
        if (string.IsNullOrEmpty(query)) return Users;
        var lowerQuery = query.ToLower();

        return Users.Where(u => u.MiddleName.ToLower().Contains(lowerQuery));
    }
}