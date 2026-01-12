using Microsoft.AspNetCore.SignalR.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5003/testHub") // host üzerinden bağlan
    .WithAutomaticReconnect()
    .Build();

connection.On<string, string>("ReceiveMessage", (user, message) =>
{
    Console.WriteLine($"{user} says: {message}");
});


await connection.StartAsync();
Console.WriteLine("Connected to TestHub");

// Server’a mesaj gönder
await connection.InvokeAsync("SendMessage", "ClientUser", "Hello from client");


app.Run();
