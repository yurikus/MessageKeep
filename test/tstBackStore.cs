using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using NSubstitute;

using MessageKeep;
using MessageKeep.Types;
using MessageKeep.Core;

namespace MessageKeep.Tests
{
    public class tstBackStore
    {
        public tstBackStore()
        {
        }

        static IEnumerable<string[]> UserChannelData()
        {
            yield return new string[]
            {
                "user-1", "channel-1"
            };

            yield return new string[]
            {
                "user-2", "channel-1"
            };

            yield return new string[]
            {
                "user-3", "channel-2"
            };
        }

        [Theory]
        [MemberData(nameof(UserChannelData))]
        public void subscribe_should_add_channel_and_user(string username_, string channel_)
        {
            IBackStore store = new BackStore();

            Assert.DoesNotContain(channel_, store.Channels);
            Assert.DoesNotContain(username_, store.Users);
            Assert.DoesNotContain(username_, store.ChannelUsers(channel_));

            store.Subscribe(username_, channel_);

            Assert.Contains(channel_, store.Channels);
            Assert.Contains(username_, store.Users);
            Assert.Contains(username_, store.ChannelUsers(channel_));
        }

        [Fact]
        public void unsubscribe_should_remove_user()
        {
            IBackStore store = new BackStore();

            var data = UserChannelData().ToList();
            var u2ch1 = data[1];

            foreach (var pair in data)
                store.Subscribe(pair[0], pair[1]);

            Assert.Contains(u2ch1[0], store.ChannelUsers(u2ch1[1]));

            var ret = store.UnSubscribe(u2ch1[0], u2ch1[1]);

            Assert.Equal(eOpStatus.Ok, ret.code);
            Assert.DoesNotContain(u2ch1[0], store.ChannelUsers(u2ch1[1]));
        }

        [Fact]
        public void mismatched_unsubscribe_should_return_Ok()
        {
            IBackStore store = new BackStore();

            var data = UserChannelData().ToList();
            var u2ch1 = data[1];

            foreach (var pair in data)
                store.Subscribe(pair[0], pair[1]);

            Assert.DoesNotContain(u2ch1[0], store.ChannelUsers("dummy-chan"));

            var ret = store.UnSubscribe(u2ch1[0], "dummy-chan");

            Assert.Equal(eOpStatus.Ok, ret.code);
        }

        [Fact]
        public void direct_message_creates_users()
        {
            IBackStore store = new BackStore();

            var data = UserChannelData().ToList();
            var u1ch1 = data[0];
            var u2ch1 = data[1];

            Assert.DoesNotContain(u1ch1[0], store.Users);
            Assert.DoesNotContain(u2ch1[0], store.Users);

            store.PushDirect(u2ch1[0], u1ch1[0], "direct message");

            Assert.Contains(u1ch1[0], store.Users);
            Assert.Contains(u2ch1[0], store.Users);
        }

        [Theory]
        [InlineData("user-1", "channel-1")]
        public void broadcast_into_subbed_chan_returns_Ok(string username_, string channel_)
        {
            IBackStore store = new BackStore();

            Assert.DoesNotContain(username_, store.Users);
            Assert.DoesNotContain(username_, store.ChannelUsers(channel_));

            store.Subscribe(username_, channel_);
            Assert.Contains(username_, store.ChannelUsers(channel_));

            var ret = store.PushBroadcast(username_, channel_, "broadcast message");

            Assert.Equal(eOpStatus.Ok, ret.code);
        }

        [Theory]
        [InlineData("user-1", "channel-1")]
        public void mismatched_broadcast_returns_NotSubscribed(string username_, string channel_)
        {
            IBackStore store = new BackStore();

            Assert.DoesNotContain(username_, store.Users);
            Assert.DoesNotContain(username_, store.ChannelUsers(channel_));

            var ret = store.PushBroadcast(username_, channel_, "broadcast message");

            Assert.DoesNotContain(username_, store.ChannelUsers(channel_));

            Assert.Equal(eOpStatus.NotSubScribed, ret.code);
        }
    }
}
