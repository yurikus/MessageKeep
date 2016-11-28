using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessageKeep.Types;

namespace MessageKeep.Core
{
    class BackStore : IBackStore
    {
        // msgId => set<message>
        readonly ConcurrentDictionary<int, HashSet<IMessage>> m_messages;

        // channel => set<username>
        readonly ConcurrentDictionary<string, HashSet<string>> m_chanSubs;

        public BackStore()
        {
            m_messages = new ConcurrentDictionary<int, HashSet<IMessage>>();
            m_chanSubs = new ConcurrentDictionary<string, HashSet<string>>();
        }

        public OpStatus Subscribe(string username_, string channel_)
        {
            var users = m_chanSubs.GetOrAdd(channel_, _ => new HashSet<string>());

            lock (users)
                users.Add(username_);

            return OpStatus.Ok;
        }

        public OpStatus UnSubscribe(string username_, string channel_)
        {
            HashSet<string> users = null;
            if (m_chanSubs.TryGetValue(channel_, out users))
            {
                lock (users)
                    users.Remove(username_);
            }

            return OpStatus.Ok;
        }

        public OpStatus PushDirect(string username_, string author_, string content_)
        {

            return OpStatus.Ok;
        }

        public OpStatus PushBroadcast(string channel_, string author_, string content_)
        {
            return OpStatus.Ok;
        }
    }
}
