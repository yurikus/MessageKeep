using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommandLine;

namespace MessageKeep
{
    public class CmdLineOptions
    {
        [Option("port", Required = true, HelpText = "Port to listen on.")]
        public ushort Port { get; set; }

        [Option("public", HelpText = "Listen on all interfaces. Requires elevation or urlacl registration. By default we listen on localhost.")]
        public bool IsPublic { get; set; }
    }
}
