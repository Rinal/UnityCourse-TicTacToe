using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using TicTacToe.Shared;

namespace TicTacToe.Server
{
    public sealed class GameHub : Hub<IGameHub>
        , IJoinOperation
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

        public async Task Join(string json)
        {
            m_logger.LogInformation($"Join with Json {json}");
            await Clients.All.OnFieldChanged("This is future json!");
        }
    }
}