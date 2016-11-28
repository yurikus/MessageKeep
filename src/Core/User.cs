using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessageKeep.Types;

namespace MessageKeep.Core
{
    class UserStore : IUserStore
    {
        readonly ConcurrentDictionary<string, IUser> m_users;

        public UserStore()
        {
            m_users = new ConcurrentDictionary<string, IUser>();
        }

        public IUser Get(string username_)
        {
            return m_users.GetOrAdd(username_, itm => new User(username_));
        }

        public IEnumerable<IUser> List()
        {
            throw new NotImplementedException();
        }
    }

    class User : IUser
    {
        readonly object SyncRoot = new object();
        readonly List<IChannel> m_channels = new List<IChannel>();
        readonly Dictionary<int, IMessage> m_messages = new Dictionary<int, IMessage>();

        public User(string username_)
        {
            Username = username_;
        }

        public string Username { get; private set; }

        public void Subscribe(IChannel channel_)
        {
            lock (SyncRoot)
            {
                m_channels.Add(channel_);
                channel_.Add(this);
            }
        }

        public void Unsubscribe(IChannel channel_)
        {
            lock (SyncRoot)
            {
                channel_.Remove(this);
                m_channels.Remove(channel_);
            }
        }

        public void Receive(IMessage msg_)
        {
            lock (SyncRoot)
            {
                m_messages.Add(msg_.Id, msg_);
                msg_.MarkReceived();
            }
        }
    }
}
