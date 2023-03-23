using Microsoft.AspNetCore.SignalR;
using WebTopOdin.Context;
using WebTopOdin.Models;

namespace WebTopOdin.Hubs;

//!!!Требуется пакет Microsoft.AspNetCore.SignalR!!!
//Класс, предоставляющий операции для взаимосвязи с клиентом
public class StrazhnikProHub : Hub
{
    private StrazhnikProContext _db;

    public StrazhnikProHub()
    {
        _db = new StrazhnikProContext();
    }
    
    public IEnumerable<User> GetUsers()
    {
        return _db.Users.ToList();
    }
}