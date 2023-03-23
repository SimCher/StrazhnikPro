using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace DeskTopOdin.Services;

public class HubService
{
    public bool IsConnected => _hubConnection is {State: HubConnectionState.Connected};
    private HubConnection _hubConnection;

    private readonly string _ip = @"http://127.0.0.1:5253/hub";

    public HubService()
    {
        InitializeServer(_ip);
        Debug.WriteLine("Server is initialized!");
    }

    public async Task ConnectAsync()
    {
        if (IsConnected) return;
        await _hubConnection.StartAsync();
        Debug.WriteLine("Connection is start!");
    }

    public async Task DisconnectAsync()
    {
        if (!IsConnected) return;

        await _hubConnection.DisposeAsync();
    }

    public async Task<IEnumerable<Models.User>> GetUsersAsync()
    {
        Debug.WriteLine("Connecting...");
        await ConnectAsync();
        Debug.WriteLine("Try to invoke GetUsers from server...");
        var users = await _hubConnection.InvokeAsync<IEnumerable<Models.User>>("GetUsers");
        Debug.WriteLine("Done!");
        return users;
    }

    private void InitializeServer(string ip)
    {
        //Создаём подключение
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(ip)
            .Build();
    }
}