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
        // username => user's messages
        readonly ConcurrentDictionary<string, HashSet<IMessage>> m_messages;

        // channel => channel users
        readonly ConcurrentDictionary<string, HashSet<string>> m_chanSubs;

        public BackStore()
        {
            m_messages = new ConcurrentDictionary<string, HashSet<IMessage>>();
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
            var msgs = m_messages.GetOrAdd(username_, _ => new HashSet<IMessage>());

            lock (msgs)
            {
                var msg = new Message(author_, content_);
                msgs.Add(msg);
                msg.MarkDelivered();
            }

            return OpStatus.Ok;
        }

        public OpStatus PushBroadcast(string channel_, string author_, string content_)
        {
            HashSet<string> users = null;
            if (!m_chanSubs.TryGetValue(channel_, out users))
                return OpStatus.NotSubscribed;

            string[] usersLocal = null;
            lock (users)
            {
                if (!users.Contains(author_))
                    return OpStatus.NotSubscribed;

                users.CopyTo(usersLocal);
            }

            foreach (var user in usersLocal)
            {
                var msgs = m_messages.GetOrAdd(user, _ => new HashSet<IMessage>());
                lock (msgs)
                {
                    var msg = new Message(author_, content_);
                    msgs.Add(msg);
                    msg.MarkDelivered();
                }
            }

            return OpStatus.Ok;
        }
    }
}
