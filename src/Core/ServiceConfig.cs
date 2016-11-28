using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessageKeep.Types;

namespace MessageKeep.Core
{
    class ServiceConfig : IServiceConfig
    {
        public string bitness => Environment.Is64BitProcess ? "x64" : "x86";
    }
}
