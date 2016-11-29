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
        readonly HashSet<string> m_users;

        // username => user's messages
        readonly ConcurrentDictionary<string, HashSet<IMessage>> m_messages;

        // channel => channel users
        readonly ConcurrentDictionary<string, HashSet<string>> m_chanSubs;

        public BackStore()
        {
            m_users = new HashSet<string>();
            m_messages = new ConcurrentDictionary<string, HashSet<IMessage>>();
            m_chanSubs = new ConcurrentDictionary<string, HashSet<string>>();
        }

        public IList<string> Users
        {
            get
            {
                // We want to avoid long lock on the users collection.
                // We're also reasonable sure that locking on the object itself
                // is side-effect-free, since we own it and don't give refs out.

                lock (m_users)
                {
                    var usersLocal = new string[m_users.Count];
                    m_users.CopyTo(usersLocal);
                    return usersLocal;
                }
            }
        }

        public IList<string> Channels
        {
            get
            {
                // ConcurrentDictionary returns snapshots for Keys and Values
                return m_chanSubs.Keys.ToList();
            }
        }

        public IList<string> ChannelUsers(string channel_)
        {
            HashSet<string> users = null;
            if (!m_chanSubs.TryGetValue(channel_, out users))
                return Enumerable.Empty<string>().ToList();

            string[] usersLocal = null;
            lock (m_users)
            {
                usersLocal = new string[users.Count];
                users.CopyTo(usersLocal);
                return usersLocal;
            }
        }

        public OpStatus Subscribe(string username_, string channel_)
        {
            var users = m_chanSubs.GetOrAdd(channel_, _ => new HashSet<string>());

            lock (m_users)
            {
                m_users.Add(username_);
                users.Add(username_);
            }

            return OpStatus.Ok;
        }

        public OpStatus UnSubscribe(string username_, string channel_)
        {
            HashSet<string> users = null;
            if (m_chanSubs.TryGetValue(channel_, out users))
            {
                lock (m_users)
                {
                    m_users.Remove(username_);
                    users.Remove(username_);
                }
            }

            return OpStatus.Ok;
        }

        public OpStatus PushDirect(string username_, string author_, string content_)
        {
            var msgs = m_messages.GetOrAdd(username_, _ => new HashSet<IMessage>());
            lock (msgs)
            {
                // deadlock warning: lock(msgs) when locked on m_users
                lock (m_users)
                {
                    m_users.Add(username_);
                    m_users.Add(author_);
                }

                var msg = new Message(author_, content_);
                msgs.Add(msg);
                msg.MarkDelivered();
            }

            return OpStatus.Ok;
        }

        public OpStatus PushBroadcast(string author_, string channel_, string content_)
        {
            HashSet<string> users = null;
            if (!m_chanSubs.TryGetValue(channel_, out users))
                return OpStatus.NotSubscribed;

            string[] usersLocal = null;
            lock (m_users)
            {
                usersLocal = new string[users.Count];
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
