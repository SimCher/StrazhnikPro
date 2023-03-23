using WebTopOdin.Context;
using WebTopOdin.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();

//============================================================
//Добавление класса контекста БД
builder.Services.AddDbContext<StrazhnikProContext>();
//============================================================

//============================================================
//Добавляем SignalR - для упрощённой связи с сервером и клиентом
builder.Services.AddSignalR();
//============================================================

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

//Подключаем хаб, который будет связывать сервер с клиентом
//по пути http://адрес_api/hub
app.MapHub<StrazhnikProHub>("/hub");

app.Run();