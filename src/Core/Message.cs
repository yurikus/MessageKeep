using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessageKeep.Types;

namespace MessageKeep.Core
{
    class Message : IMessage
    {
        readonly uint m_id;
        readonly string m_sender;
        readonly string m_content;
        readonly string m_recipient;
        readonly bool m_isDirect;
        DateTime m_deliveredUtc = DateTime.MinValue;

        public Message(string sender_, string content_, string recipient_, bool isDirect_)
        {
            m_sender = sender_;
            m_content = content_;
            m_recipient = recipient_;
            m_isDirect = isDirect_;

            unchecked
            {
                m_id = 17;
                m_id = m_id * 23 + (uint) m_sender.GetHashCode();
                m_id = m_id * 23 + (uint) m_recipient.GetHashCode();
                m_id = m_id * 23 + (uint) m_isDirect.GetHashCode();
            }
        }

        public uint Id => m_id;
        public string Sender => m_sender;
        public string Content => m_content;
        public string Recipient => m_recipient;
        public bool IsDirect => m_isDirect;
        public DateTime DeliveredUtc => m_deliveredUtc;

        public void MarkDelivered()
        {
            if (m_deliveredUtc != DateTime.MinValue)
                return;

            m_deliveredUtc = DateTime.UtcNow;
        }

        public override int GetHashCode()
        {
            return (int) m_id;
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
