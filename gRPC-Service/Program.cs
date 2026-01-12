using gRPC_Service.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

// Reflection servisini ekle
builder.Services.AddGrpcReflection();

var app = builder.Build();

app.MapGrpcService<TestServiceImpl>();

// Reflection endpoint’i sadece development ortamında açmak güvenli
app.MapGrpcReflectionService();

app.MapGet("/", () => "Use a gRPC client to communicate with this service.");

app.Run();