using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AvaloniaApplication1.Models;
using Newtonsoft.Json;

namespace AvaloniaApplication1.Services;

//Служебный класс, связывающий приложение с API
public class ApiService
{
    //1. Создаём объект типа HttpClient, который будет связываться с API
    private HttpClient _httpClient;

    //
    private readonly string _ip = "http://127.0.0.1:5291/";
    private readonly string _usersPath = "users";
    private readonly string _usersAddPath = "users/add";
    private readonly string _usersUpdPath = "users/update/";
    private readonly string _usersDeletePath = "users/delete/";

    public ApiService()
    {
        _httpClient = new HttpClient();
    }
    
    public IEnumerable<User>? Users { get; private set; }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        var response = await _httpClient.GetAsync($"{_ip}{_usersPath}");

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            Users = JsonConvert.DeserializeObject<IEnumerable<User>>(jsonString);
        }
        
        return Users;
    }

    public User GetUserById(int id)
    {
        return Users.First(u => u.Id == id);
    }

    public async Task AddUserAsync(User user)
    {
        var json = JsonConvert.SerializeObject(user);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_ip}{_usersAddPath}", data);
    }

    public async Task UpdateUserAsync(User user)
    {
        var json = JsonConvert.SerializeObject(user);
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync($"{_ip}{_usersUpdPath}{user.Id}", data);
    }

    public async Task DeleteUserAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{_ip}{_usersDeletePath}{id}");
    }
    
    
}