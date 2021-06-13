using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using PdfOcr.Services;

namespace PdfOcr.SignalR
{
    public class MyMessageHub : Hub
    {
        private readonly IOcr _ocr;
        private readonly ILogger<MyMessageHub> _logger;

        public MyMessageHub(IOcr ocr, ILogger<MyMessageHub> logger)
        {
            _ocr = ocr;
            _logger = logger;
        }
        public override Task OnConnectedAsync()
        {
            Console.WriteLine("Connected");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _logger.LogInformation($"Connection lost with {Context.ConnectionId}");
            //_ocr.KillProcess(Context.ConnectionId);
            _ocr.KillProcess(Context);
            return base.OnDisconnectedAsync(exception);
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

        public bool StopConversion()
        {
            return _ocr.KillProcess(Context);
        }
    }
}