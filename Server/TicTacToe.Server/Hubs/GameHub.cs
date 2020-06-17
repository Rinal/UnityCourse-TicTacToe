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
        private readonly ICheckableField m_checkableField = default;
        private readonly ISettableField m_settableField = default;
        private readonly IAnalysis m_analysis = default;
        private readonly IFieldElements m_fieldElements = default;

        public GameHub(
            ILogger<GameHub> logger,
            IUsersState usersState,
            IActiveUserState activeUserState,
            IEnumerable<UserModel> users,
            ICheckableField checkableField,
            ISettableField settableField,
            IAnalysis analysis,
            IFieldElements fieldElements)
        {
            m_logger = logger;
            m_usersState = usersState;
            m_activeUserState = activeUserState;
            m_users = users;
            m_checkableField = checkableField;
            m_settableField = settableField;
            m_analysis = analysis;
            m_fieldElements = fieldElements;
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
            UserModel second = m_users.ElementAtOrDefault(1);
            if (second != null)
            {
                m_activeUserState.Set(second.Id);
                await Clients.All.OnStateChanged(new ActiveUserChangedEvent(m_activeUserState.Current).ToEvent());
            }

            return new JoinOperationResponse().ToJson();
        }

        public async Task<string> Select(string json)
        {
            SelectOperationRequest request = JsonConvert.DeserializeObject<SelectOperationRequest>(json);
            m_logger.LogInformation($"User {Context.ConnectionId} going to select {request}");
            if (!m_checkableField.IsEmpty(request.X, request.Y))
            {
                return new SelectOperationResponse($"Invalid values! Filed is not empty at ({request.X},{request.Y})").ToJson();
            }

            //TODO Check if user is active!
            UserModel currentUser = m_usersState[Context.ConnectionId];
            if (!currentUser.Id.Equals(m_activeUserState.Current))
            {
                return new SelectOperationResponse($"Failed! User {currentUser.Id} active!").ToJson();
            }

            if (currentUser.Symbol != request.Symbol)
            {
                return new SelectOperationResponse($"Invalid symbol for user {currentUser.Id}!").ToJson();
            }

            //Changing the game field
            m_settableField.Set(request.Symbol, request.X, request.Y);
            await Clients.All.OnStateChanged(new FieldChangedEvent(request.Symbol, request.X, request.Y).ToEvent());

            //Changing the active user
            UserModel otherUser = m_users.FirstOrDefault(u => !u.Id.Equals(currentUser.Id));
            if (otherUser == null)
            {
                return new SelectOperationResponse("Failed to find other user!").ToJson();
            }

            m_activeUserState.Set(otherUser.Id);
            await Clients.All.OnStateChanged(new ActiveUserChangedEvent(m_activeUserState.Current).ToEvent());
            if (m_analysis.WinnerDefiner(m_fieldElements.Field, out Symbols? symbol))
            {
                m_logger.LogInformation($"The game is over. Winner {symbol}");
                await Clients.All.OnStateChanged(new GameOverEvent(symbol).ToEvent());
            }

            return new SelectOperationResponse().ToJson();
        }
    }
}