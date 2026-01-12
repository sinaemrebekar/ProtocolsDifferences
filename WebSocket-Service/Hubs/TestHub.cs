using Microsoft.AspNetCore.SignalR;

namespace WebSocket_Service.Hubs
{
  public class TestHub : Hub
  {
    public async Task SendMessage(string user, string message)
    {
      await Clients.Caller.SendAsync("ReceiveMessage", user, message);
    }
  }
}
