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

    //2. Сохраняем все пути в переменные, чтобы не прописывать их каждый раз заново
    private readonly string _ip = "http://127.0.0.1:5291/";
    private readonly string _usersPath = "users";
    private readonly string _usersAddPath = "users/add";
    private readonly string _usersUpdPath = "users/update/";
    private readonly string _usersDeletePath = "users/delete/";

    public ApiService()
    {
        //3. Инициализируем
        _httpClient = new HttpClient();
    }
    
    //4. Тут делаем на всякий случай сохранение всех пользователей, чтобы производительность не страдала
    public IEnumerable<User>? Users { get; private set; }
    
    /// <summary>
    /// Выгрузить список пользователей с сервера
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        //Пытаемся обратиться к API
        var response = await _httpClient.GetAsync($"{_ip}{_usersPath}");

        //Если API вернул успешный ответ
        if (response.IsSuccessStatusCode)
        {
            //Получаем данные в формате JSON, которые вернуло API
            var jsonString = await response.Content.ReadAsStringAsync();
            //Преобразуем JSON-строку в объекты типа User и сохраняем их в свойство этого класса
            Users = JsonConvert.DeserializeObject<IEnumerable<User>>(jsonString);
        }
        
        return Users;
    }

    /// <summary>
    /// Передаёт данные API для добавления нового пользователя
    /// </summary>
    /// <param name="user"></param>
    public async Task AddUserAsync(User user)
    {
        //преобразуем данные нового пользователя в JSON для передачи к API
        var json = JsonConvert.SerializeObject(user);
        //Указываем в какой кодировке будем передавать данные API
        var data = new StringContent(json, Encoding.UTF8, "application/json");

        //Указываем, какой будем использовать метод (POST)
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