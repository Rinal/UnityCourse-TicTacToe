using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace TicTacToe.Server
{
    public sealed class GameHub : Hub
    {
        private readonly ILogger<GameHub> m_logger = default;

        public GameHub(ILogger<GameHub> logger)
        {
            m_logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            m_logger.LogInformation($"User connected: Connection={Context.ConnectionId}, UserId={Context.UserIdentifier}");
            await base.OnConnectedAsync();
        }
    }
}