using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace PdfOcr.SignalR
{
    public class MyMessageHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine("Connected");
            return base.OnConnectedAsync();
        }

        public async Task BroadcastMessage(string message)
        {
            await Clients.All.SendAsync("broadcastmessage", message);
        }

        public string GetConnectionId()
        {
            Console.WriteLine($"ConnectionId: {Context.ConnectionId}");
            return Context.ConnectionId;
        }
    }
}