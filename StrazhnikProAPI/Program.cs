using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using StrazhnikProAPI.Context;
using StrazhnikProAPI.Models;

var builder = WebApplication.CreateBuilder(args);

//Обязательно добавляем сюда контекст базы данных!
builder.Services.AddDbContext<StrazhnikProContext>();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

//=================== Точки по которым клиентские приложения смогут получать данные с сервера ==========================

//Точка для получения (Get) всех пользователей (просто возвращаем список пользователей)
app.MapGet("/users", (StrazhnikProContext db) => db.Users.ToList());

//Точка для получения конкретного пользователя по Id
app.MapGet("/users/{id}", (StrazhnikProContext db, int id) =>
{
    var user = db.Users.FindAsync(id);
    return Results.Json(user);
});


//Точка для добавления (Post) нового пользователя (аргументы: контекст БД, Http-контекст, которые содержит данные, отправленные от клиента)
app.MapPost("/users/add", async (StrazhnikProContext db, HttpContext context) =>
{
    //Создаём поток для чтения данных, отправленных с клиента, где UTF8 - указываем в какой кодировке переданы данные
    var reader = new HttpRequestStreamReader(context.Request.Body, Encoding.UTF8);
    //Считываем все данные, которые расшифровал поток
    var body = await reader.ReadToEndAsync();
    
    //Преобразуем JSON-объект в объект типа User 
    var user = JsonConvert.DeserializeObject<User>(body);
    //Добавляем пользователя в базу
    await db.AddUserAsync(user);
    //Сообщаем клиенту, что всё успешно
    context.Response.StatusCode = 200;
});

//Точка для изменения (Put) существующего пользователя
app.MapPut("users/update/{id}", async (StrazhnikProContext db, HttpContext context, int id) =>
{
    var reader = new HttpRequestStreamReader(context.Request.Body, Encoding.UTF8);
    var body = await reader.ReadToEndAsync();
    var userWithNewData = JsonConvert.DeserializeObject<User>(body);

    var existingUser = await db.Users.FindAsync(id);

    if (existingUser != null)
    {
        existingUser.FirstName = userWithNewData.FirstName;
        existingUser.MiddleName = userWithNewData.MiddleName;
        existingUser.LastName = userWithNewData.LastName;
        
        await db.SaveChangesAsync();

        context.Response.StatusCode = 200;
    }
    else
    {
        context.Response.StatusCode = 404;
    }

});

app.MapDelete("users/delete/{id}", async (StrazhnikProContext db, HttpContext context, int id) =>
{
    var userToDelete = await db.Users.FindAsync(id);

    if (userToDelete == null) return Results.NotFound(new {message = $"No user with id: {id}"});

    db.Users.Remove(userToDelete);
    await db.SaveChangesAsync();
    return Results.Ok();
});

//======================================================================================================================

app.Run();