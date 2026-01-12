using Grpc.Net.Client;
using GrpcService;
using Microsoft.AspNetCore.SignalR.Client;
using Prometheus;
using System.Diagnostics;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAll",
      policy =>
      {
        policy
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
      });
});

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddHttpClient("rest", client =>
{
  client.Timeout = TimeSpan.FromSeconds(30);
});
var app = builder.Build();
app.UseCors("AllowAll");
// PROMETHEUS – endpoint öncesi
app.UseHttpMetrics();


// Health check
app.MapGet("/health", () =>
{
  Console.WriteLine($"Orchestrator is OK");
  return Results.Ok("Orchestrator is healthy");
});

// METRICS
app.MapMetrics();

// REST load test
app.MapPost("/test/rest", async (
    int payloadSize,
    IHttpClientFactory httpFactory) =>
{
  Console.WriteLine(
      $"Starting REST load test payload size {payloadSize} bytes");

  var client = httpFactory.CreateClient("rest");

  var payload = new string('x', payloadSize);
  var content = new StringContent(payload, Encoding.UTF8, "application/json");
  var sw = Stopwatch.StartNew();

  var response = await client.PostAsync(
      $"http://rest-service:8081/test", content);

  sw.Stop();

  return Results.Ok(new
  {
    totalRequests = 1,
    payloadSize,
    elapsedMs = sw.ElapsedMilliseconds
  });
});


// gRPC metrics
var grpcCallDuration = Metrics.CreateHistogram("grpc_call_duration_seconds", "gRPC call duration");
var grpcCallCounter = Metrics.CreateCounter("grpc_call_total", "Total gRPC calls");

app.MapPost("/test/grpc", async (int payloadSize) =>
{
  Console.WriteLine(
      $"Starting gRPC load test payload size {payloadSize}");

  using var channel = GrpcChannel.ForAddress("http://grpc-service:5002");
  var client = new TestService.TestServiceClient(channel);


  var payload = new string('x', payloadSize);
  var sw = Stopwatch.StartNew();

  await client.GetTestDataAsync(new TestRequest
  {
    Id = new Random().Next(0, 10000),
    Payload = payload
  });

  sw.Stop();

  grpcCallDuration.Observe(sw.Elapsed.TotalSeconds);
  grpcCallCounter.Inc();

  return Results.Ok(new
  {
    request = 1,
    payloadSize,
    elapsedMs = sw.ElapsedMilliseconds
  });
});



// WebSocket Load Test
// ---------------------------
var wsCallDuration = Metrics.CreateHistogram("ws_call_duration_seconds", "WebSocket call duration");
var wsCallCounter = Metrics.CreateCounter("ws_call_total", "Total WebSocket calls");

app.MapPost("/test/websocket", async (int payloadSize) =>
{
  Console.WriteLine($"Starting WebSocket load test payload size {payloadSize}");


  var payload = new string('x', payloadSize);


  var sw = Stopwatch.StartNew();

  var connection = new HubConnectionBuilder()
      .WithUrl("ws://websocket-service:8083/testHub")
      .Build();

  await connection.StartAsync();

  await connection.InvokeAsync(
      "SendMessage",
      $"User{new Random().Next(0, 100000)}",
      payload
  );

  await connection.StopAsync();
  await connection.DisposeAsync();

  sw.Stop();
  wsCallDuration.Observe(sw.Elapsed.TotalSeconds);
  wsCallCounter.Inc();

  return Results.Ok(new
  {
    request = 1,
    payloadSize,
    elapsedMs = sw.ElapsedMilliseconds
  });
});


app.Run();
