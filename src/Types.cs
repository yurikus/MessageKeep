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
        IList<IMessage> ChannelMessages(string channel_);
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

/*

+   GET      /users                                       list of users
+   GET      /users/<user>                                summary of user's subbed channels and dm
    
+   GET      /users/<user>/messages                       user's messages (direct or broadcast)
+   GET      /users/<user>/messages/to/<user2>            list of user messages to user2
+   POST     /users/<user>/messages/to/<user2>            post message to user2
    
+   PUT      /users/<user>/channel/<chan>                 subscribe user to channel
+   DELETE   /users/<user>/channel/<chan>                 unsubscibe user from channel
+   POST     /users/<user>/channel/<chan>                 post a message to channel
        
                                                           
+   GET      /channels                                    list of channels
+   GET      /channels/<chan>/users                       summary of subbed users and message count
+   GET      /channels/<chan>/messages                    broadcast messages for the channel
                                                      
*/
