using Grpc.Net.Client;
using GrpcService;

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
var channel = GrpcChannel.ForAddress("http://host.docker.internal:5002", new GrpcChannelOptions
{
    HttpHandler = new HttpClientHandler
    {
        // Gerekirse TLS olmadan
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    }
}); // Docker içindeyse IP adresi değişebilir
var client = new TestService.TestServiceClient(channel);

var reply = await client.GetTestDataAsync(new TestRequest { Id = 1 });
Console.WriteLine($"Message: {reply.Message}, Timestamp: {reply.Timestamp}");

app.Run();

