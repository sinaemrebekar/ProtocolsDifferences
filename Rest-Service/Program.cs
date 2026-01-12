var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapPost("/test", async (HttpRequest request) =>
{
  Console.WriteLine(
    $"Request received data: {request.Body}");

  using var reader = new StreamReader(request.Body);
  var payload = await reader.ReadToEndAsync();
  
  return Results.Json(new
  {
    id = 1,
    message = "Hello from REST Service",
    timestamp = DateTime.UtcNow,
    payload
  });
});

app.MapGet("/health", () =>
    Results.Ok("REST Service running...")
);

app.MapGet("/metrics", () =>
    Results.Text("rest_service_requests_total 1")
);

app.Run();
