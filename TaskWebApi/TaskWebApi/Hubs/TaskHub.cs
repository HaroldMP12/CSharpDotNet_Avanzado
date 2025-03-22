using Microsoft.AspNetCore.SignalR;

namespace TaskWebApi.Hubs
{
    public class TaskHub : Hub
    {
        public async Task SendTaskUpdate(string user, string message)
           => await Clients.All.SendAsync("ReceiveTaskUpdate", user, message);
    }
}