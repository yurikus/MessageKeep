using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageKeep.Types
{
    public interface IServiceConfig
    {
        string bitness { get; }
    }

    public interface IBackStore
    {
        OpStatus Subscribe(string username_, string channel_);
        OpStatus UnSubscribe(string username_, string channel_);
        OpStatus PushDirect(string username_, string author_, string content_);
        OpStatus PushBroadcast(string channel_, string author_, string content_);
    }

    public interface IUser
    {
        string Username { get; }
        void Receive(IMessage msg_);
        void Subscribe(IChannel channel_);
        void Unsubscribe(IChannel channel_);
    }

    public interface IChannel
    {
        string Title { get; }
        void Broadcast(IMessage msg_);
        void Add(IUser user_);
        void Remove(IUser user_);
    }

    public interface IMessage
    {
        int Id { get; }
        string Author { get; }
        string Content { get; }
        DateTime DeliveredUtc { get; }
        void MarkDelivered();
    }
}

/*

    GET      /service                                     status, runtime info
    GET      /service/config                              options
    POST     /service/config                              options
        
        
    GET      /users                                       list of users
    GET      /users/<user>                                summary of user's subbed channels and dm
    
    GET      /users/<user>/messages                       last 100 user messages (direct or broadcast)
    GET      /users/<user>/messages[?skip=<x>&count=<y>]  at most <y> user messages starting from <x> (direct or broadcast)
    GET      /users/<user>/messages/since/<date>          user messages since <date> (direct or broadcast)
    GET      /users/<user>/messages/<id>                  get pecific message
    GET      /users/<user>/messages/to/<user2>            list of user messages to user2
    POST     /users/<user>/messages/to/<user2>            post message to user2
    
+   PUT      /users/<user>/channel/<chan>                 subscribe user to channel
+   DELETE   /users/<user>/channel/<chan>                 unsubscibe user from channel
    GET      /users/<user>/channel/<chan>                 list if user messages in the channel
    POST     /users/<user>/channel/<chan>                 post a message to channel
        
                                                           
    GET      /channels                                    list of channels
    GET      /channels/<chan>                             summary of subbed users and message count
    GET      /channels/<chan>/messages                    last 100 messages for the channel
    GET      /channels/<chan>/messages/since/<date>       channel messages since <date> (direct or broadcast)
                                                      
*/
