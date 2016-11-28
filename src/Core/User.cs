using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessageKeep.Types;

namespace MessageKeep.Core
{
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
                msg_.MarkDelivered();
            }
        }

        public override int GetHashCode()
        {
            return Username.GetHashCode();
        }

        public override bool Equals(object otherObj_)
        {
            var other = otherObj_ as IUser;
            if (other == null)
                return false;

            return Username == other.Username;
        }
    }
}
