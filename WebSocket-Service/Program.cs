using WebSocket_Service.Hubs;

var builder = WebApplication.CreateBuilder(args);
// CORS Policy adını tanımla
const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// --- KRİTİK EKLENTİ: CORS Servisini Ekleme ---
// SignalR için k6 gibi farklı bir kaynaktan gelen Negotiate POST isteğini kabul etmek üzere CORS eklenmeli.
builder.Services.AddCors(options =>
{
  options.AddPolicy(name: MyAllowSpecificOrigins,
                    policy =>
                    {
                      // Normalde buraya k6'nın çalıştığı adresi (örneğin "http://localhost:5003") 
                      // veya tüm adreslere izin vermek için "*" koyulur. Yük testi için esneklik sağlanır.
                      policy.SetIsOriginAllowed(_ => true) // Test Orchestrator adresi
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials(); // Kimlik bilgilerini (Çerezler) taşımaya izin verir (Çok Önemli!)
                    });
});

// SignalR servisini ekle
builder.Services.AddSignalR(options =>
{
  options.MaximumReceiveMessageSize = 1024 * 1024; // 1 MB
});

var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);

// Hub’ı map et
app.MapHub<TestHub>("/testHub");

// Health check
app.MapGet("/", () => "WebSocket Service running...");

app.Run();