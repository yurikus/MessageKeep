using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessageKeep.Types;

namespace MessageKeep.Core
{
    class ChannelStore : IChannelStore
    {
        readonly ConcurrentDictionary<string, IChannel> m_channels;

        public ChannelStore()
        {
            m_channels = new ConcurrentDictionary<string, IChannel>();
        }

        public IChannel Get(string channel_)
        {
            return m_channels.GetOrAdd(channel_, itm => new Channel(channel_));
        }

        public IEnumerable<IChannel> List()
        {
            throw new NotImplementedException();
        }
    }

    class Channel : IChannel
    {
        readonly List<IUser> m_users = new List<IUser>();

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
            m_users.ForEach(itm => itm.Receive(msg_));
        }
    }
}
