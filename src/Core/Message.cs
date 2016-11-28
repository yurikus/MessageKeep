using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessageKeep.Types;

namespace MessageKeep.Core
{
    class MessageStore : IMessageStore
    {
        public IMessage Create(string author_, string content_)
        {
            throw new NotImplementedException();
        }

        public IMessage Get(int messageId_)
        {
            throw new NotImplementedException();
        }
    }

    class Message : IMessage
    {
        readonly int m_id = DateTime.UtcNow.GetHashCode();
        readonly string m_author;
        readonly string m_content;
        DateTime m_receivedOnUtc = DateTime.MinValue;

        public Message(string author_, string content_)
        {
            m_author = author_;
            m_content = content_;
        }

        public int Id => m_id;
        public string Author => m_author;
        public string Content => m_content;
        public DateTime ReceivedOnUtc => m_receivedOnUtc;

        public void MarkReceived()
        {
            if (m_receivedOnUtc != DateTime.MinValue)
                return;

            m_receivedOnUtc = DateTime.UtcNow;
        }
    }
}
