using System.Threading.Tasks;

namespace PdfOcr.SignalR
{
    public interface IMessageHubClient
    {
        Task BroadcastMessage(string message);
    }
}