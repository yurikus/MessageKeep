using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageKeep.Types
{
    public enum eOpStatus
    {
        Ok,
        NotSubScribed,
        InvalidArguments
    }

    public class OpStatus
    {
        public eOpStatus code;
        public string message;

        public static OpStatus Ok = new OpStatus()
        {
            code = eOpStatus.Ok,
            message = "Ok"
        };

        public static OpStatus NotSubscribed = new OpStatus()
        {
            code = eOpStatus.NotSubScribed,
            message = "User is not subscribed to this channel"
        };

        public static OpStatus InvalidArguments = new OpStatus()
        {
            code = eOpStatus.InvalidArguments,
            message = "Some request arguments are invalid"
        };
    }
}
