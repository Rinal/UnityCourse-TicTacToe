using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace TicTacToe.Server
{
    public sealed class ServerUsersState :
        IUsersState,
        IActiveUserState,
        IEnumerable<UserModel>
    {
        private readonly ILogger<ServerUsersState> m_logger = default;
        private readonly ConcurrentDictionary<string, UserModel> m_users = new ConcurrentDictionary<string, UserModel>();
        private string m_activeUser = string.Empty;

        public ServerUsersState(ILogger<ServerUsersState> logger)
        {
            m_logger = logger;
        }

        public UserModel this[string id] => m_users[id];

        public void Add(UserModel user)
        {
            if (!m_users.TryAdd(user.Id, user))
            {
                m_logger.LogCritical($"Failed to add user with id {user.Id}!");
                return;
            }

            m_logger.LogInformation($"User {user} added.");
        }

        public void Remove(string id)
        {
            if (!m_users.TryRemove(id, out _))
            {
                m_logger.LogCritical($"Failed to remove user with id {id}!");
                return;
            }

            m_logger.LogInformation($"User {id} removed.");
        }

        public IEnumerator<UserModel> GetEnumerator()
        {
            return m_users.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string Current => m_activeUser;

        public void Set(string id)
        {
            m_logger.LogInformation($"Change active user from {m_activeUser} to {id}.");
            m_activeUser = id;
        }
    }
}