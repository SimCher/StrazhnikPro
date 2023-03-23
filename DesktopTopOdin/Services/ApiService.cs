using System.Collections.Generic;
using System.Threading.Tasks;
using DesktopTopOdin.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace DesktopTopOdin.Services;

public class ApiService
{
    private HubConnection _hub;
    private readonly string _ip = @"http://127.0.0.1:5253/hub";

    public bool IsConnected => _hub is {State: HubConnectionState.Connected};

    public ApiService()
    {
        InitializeServer(_ip);
    }

    public async Task ConnectAsync()
    {
        if (IsConnected) return;
        await _hub.StartAsync();
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        await ConnectAsync();
        var users = await _hub.InvokeAsync<IEnumerable<User>>("GetUsers");
        return users;
    }

    private void InitializeServer(string ip)
    {
        _hub = new HubConnectionBuilder()
            .WithUrl(_ip)
            .Build();
    }
}