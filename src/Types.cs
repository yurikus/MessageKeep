using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageKeep.Types
{
    public interface IBackStore
    {
        IList<string> Users { get; }
        IList<string> Channels { get; }

        IList<string> ChannelUsers(string channel_);
        IList<string> UserChannels(string username_);
        IList<IMessage> UserMessages(string username_);
        IList<IMessage> UserMessagesSince(string username_, DateTime startDate_);
        IList<IMessage> ChannelMessages(string channel_);
        IList<IMessage> ChannelMessagesSince(string channel_, DateTime startDate_);
        IList<IMessage> DirectMessages(string sender_, string recipient_);

        OpStatus Subscribe(string username_, string channel_);
        OpStatus UnSubscribe(string username_, string channel_);
        OpStatus PushDirect(string sender_, string recipient_, string content_);
        OpStatus PushBroadcast(string sender_, string channel_, string content_);
    }

    public interface IMessage
    {
        uint Id { get; }
        DateTime DeliveredUtc { get; }
        bool IsDirect { get; }
        string Sender { get; }
        string Recipient { get; }
        string Content { get; }
        void MarkDelivered();
    }
}
