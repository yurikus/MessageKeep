using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessageKeep.Types;

namespace MessageKeep.Core
{
    class Channel : IChannel
    {
        readonly HashSet<IUser> m_users = new HashSet<IUser>();
        readonly HashSet<IMessage> m_messages = new HashSet<IMessage>();

        public Channel(string title_)
        {
            Title = title_;
        }

        public string Title { get; private set; }

        public void Add(IUser user_)
        {
            m_users.Add(user_);
        }

        public void Remove(IUser user_)
        {
            m_users.Remove(user_);
        }

        public void Broadcast(IMessage msg_)
        {
//            m_users.ForEach(itm => itm.Receive(msg_));
        }
    }
}
