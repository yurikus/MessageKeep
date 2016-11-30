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

        // I opted for a single message store vs message queue per user channel.
        // This simplifies things a bit by using flags on the message instance to
        // distinguish direct vs broadcast. It also comes with a couple of drawbacks: 
        // not having distict delivery timers per user and temporarily lost history
        // of messages broadcast to a channel after user unsubscribes from it.
        // User is able to get the history back by subscribing to the channel again.
        readonly HashSet<IMessage> m_messages;

        // channel => channel users
        readonly ConcurrentDictionary<string, HashSet<string>> m_chanSubs;

        public BackStore()
        {
            m_users = new HashSet<string>();
            m_messages = new HashSet<IMessage>();
            m_chanSubs = new ConcurrentDictionary<string, HashSet<string>>();
        }

        public IList<string> Users
        {
            get
            {
                // We want to avoid long lock on the users collection.
                // We're also reasonably sure that locking on the object itself
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

        public IList<string> UserChannels(string username_)
        {
            return m_chanSubs
                .Where(itm => itm.Value.Contains(username_))
                .Select(itm => itm.Key).ToList();
        }

        public IList<IMessage> UserMessages(string username_)
        {
            var channels = UserChannels(username_);

            return m_messages
                .Where(msg =>
                {
                    if (msg.IsDirect)
                    {
                        if (msg.Recipient == username_)
                            return true;
                    }
                    else
                    {
                        if (channels.Contains(msg.Recipient))
                            return true;
                    }

                    return false;
                }).ToList();
        }

        public IList<IMessage> UserMessagesSince(string username_, DateTime startDate_)
        {
            var channels = UserChannels(username_);

            return m_messages
                .Where(msg =>
                {
                    if (msg.DeliveredUtc < startDate_)
                        return false;

                    if (msg.IsDirect)
                    {
                        if (msg.Recipient == username_)
                            return true;
                    }
                    else
                    {
                        if (channels.Contains(msg.Recipient))
                            return true;
                    }

                    return false;
                }).ToList();
        }

        public IList<IMessage> ChannelMessages(string channel_)
        {
            return m_messages
                .Where(msg => !msg.IsDirect && msg.Recipient == channel_)
                .ToList();
        }

        public IList<IMessage> ChannelMessagesSince(string channel_, DateTime startDate_)
        {
            return m_messages
                .Where(msg => !msg.IsDirect && msg.Recipient == channel_ && msg.DeliveredUtc >= startDate_)
                .ToList();
        }

        public IList<IMessage> DirectMessages(string sender_, string recipient_)
        {
            return m_messages
                .Where(msg => msg.IsDirect && msg.Sender == sender_ && msg.Recipient == recipient_)
                .ToList();
        }

        public OpStatus Subscribe(string username_, string channel_)
        {
            var channelUsers = m_chanSubs.GetOrAdd(channel_, _ => new HashSet<string>());

            lock (m_users)
            {
                m_users.Add(username_);
                channelUsers.Add(username_);
            }

            return OpStatus.Ok;
        }

        public OpStatus UnSubscribe(string username_, string channel_)
        {
            HashSet<string> channelUsers = null;
            if (m_chanSubs.TryGetValue(channel_, out channelUsers))
            {
                lock (m_users)
                {
                    m_users.Remove(username_);
                    channelUsers.Remove(username_);
                }
            }

            return OpStatus.Ok;
        }

        public Tuple<OpStatus, uint> PushDirect(string sender_, string recipient_, string content_)
        {
            lock (m_users)
            {
                m_users.Add(sender_);
                m_users.Add(recipient_);
            }

            // We're reasonably sure that locking on the object itself
            // is side-effect-free, since we own it and don't give refs out.
            var msg = new Message(sender_, content_, recipient_, true);
            lock (m_messages)
            {
                m_messages.Add(msg);
                msg.MarkDelivered();
            }

            return Tuple.Create(OpStatus.Ok, msg.Id);
        }

        public Tuple<OpStatus, uint> PushBroadcast(string sender_, string channel_, string content_)
        {
            var channelUsers = ChannelUsers(channel_);
            if (!channelUsers.Contains(sender_))
                return Tuple.Create(OpStatus.NotSubscribed, (uint) 0);

            var msg = new Message(sender_, content_, channel_, false);
            lock (m_messages)
            {
                m_messages.Add(msg);
                msg.MarkDelivered();
            }

            return Tuple.Create(OpStatus.Ok, msg.Id);
        }
    }
}
