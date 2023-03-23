using System.Collections.Generic;
using System.Threading.Tasks;
using AvaloniaApplication1.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace AvaloniaApplication1.Services;

//Служебный класс, связывающий приложение с API
public class ApiService
{
    //объект для подключения к API
    private HubConnection _hub;
    
    //указываем IP-адрес API с сссылкой на хаб
    private readonly string _ip = @"http://127.0.0.1:5253/hub";
    
    public bool IsConnected => _hub is {State: HubConnectionState.Connected};

    public ApiService()
    {
        InitializeServer(_ip);
    }

    public async Task ConnectAsync()
    {
        if (IsConnected) return;
        
        //подключение к API
        await _hub.StartAsync();
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        await ConnectAsync();
        
        //Все методы, которые ты создала в API вызываем через InvokeAsync и в скобках строкой указываем
        //название метода, которое ты указала в API-приложении
        var users = await _hub.InvokeAsync<IEnumerable<User>>("GetUsers");
        
        
        return users;
    }

    private void InitializeServer(string ip)
    {
        //Обязательный метод для инициализации подключения
        _hub = new HubConnectionBuilder()
            .WithUrl(ip)
            .Build();
    }
}