using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessageKeep.Types;

namespace MessageKeep.Core
{
    class Message : IMessage
    {
        readonly int m_id = DateTime.UtcNow.GetHashCode();
        readonly string m_author;
        readonly string m_content;
        DateTime m_deliveredUtc = DateTime.MinValue;

        public Message(string author_, string content_)
        {
            m_author = author_;
            m_content = content_;
        }

        public int Id => m_id;
        public string Author => m_author;
        public string Content => m_content;
        public DateTime DeliveredUtc => m_deliveredUtc;

        public void MarkDelivered()
        {
            if (m_deliveredUtc != DateTime.MinValue)
                return;

            m_deliveredUtc = DateTime.UtcNow;
        }

        public override int GetHashCode()
        {
            return m_id;
        }

        public override bool Equals(object otherObj_)
        {
            var other = otherObj_ as IMessage;
            if (other == null)
                return false;

            return Id == other.Id;
        }
    }
}
