using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TicTacToe.Shared;

namespace TicTacToe.Server
{
    public sealed class GameHub : Hub<IGameHub>
        , IJoinOperation
        , ISelectOperation
    {
        private readonly ILogger<GameHub> m_logger = default;
        private readonly IUsersState m_usersState = default;
        private readonly IActiveUserState m_activeUserState = default;
        private readonly IEnumerable<UserModel> m_users = default;

        public GameHub(
            ILogger<GameHub> logger,
            IUsersState usersState,
            IActiveUserState activeUserState,
            IEnumerable<UserModel> users)
        {
            m_logger = logger;
            m_usersState = usersState;
            m_activeUserState = activeUserState;
            m_users = users;
        }

        public override async Task OnConnectedAsync()
        {
            m_logger.LogInformation($"User connected: {nameof(Context.ConnectionId)} = {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            m_logger.LogInformation($"User disconnected: {nameof(Context.ConnectionId)} = {Context.ConnectionId}, with {exception}");
            m_usersState.Remove(Context.ConnectionId);
            await Clients.All.OnStateChanged(m_users.ToEvent());
        }

        public async Task<string> Join(string json)
        {
            m_logger.LogInformation($"Going to Join a user {json}");
            int usersCount = m_users.Count();
            if (usersCount >= RoomSize.Value)
            {
                return new JoinOperationResponse($"Join failed due to room is full!").ToJson();
            }

            JoinOperationRequest request = JsonConvert.DeserializeObject<JoinOperationRequest>(json);
            Symbols symbol = (Symbols) usersCount;
            UserModel user = new UserModel(
                Context.ConnectionId,
                request.Name,
                symbol
            );
            m_usersState.Add(user);
            await Clients.All.OnStateChanged(m_users.ToEvent());
            return new JoinOperationResponse().ToJson();
        }

        public async Task<string> Select(string json)
        {
            SelectOperationRequest request = JsonConvert.DeserializeObject<SelectOperationRequest>(json);
            m_logger.LogInformation($"User {Context.ConnectionId} going to select {request}");
            // throw new HubException(" Thi is wrong turn!");
            await Task.CompletedTask;
            // throw new Exception("No!");
            // await Task.CompletedTask; // FromException(new HubException("NO NO"));
            // return Task.FromException(new Exception("")).Exception.ToString();
            return new SelectOperationResponse().ToJson();

            // return "This is something new!";
        }
    }
}