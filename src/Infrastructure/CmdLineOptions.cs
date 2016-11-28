using CommandLine;

namespace MessageKeep
{
    public class CmdLineOptions
    {
        [Option("port", Required = true, HelpText = "Port to listen on.")]
        public int Port { get; set; }

        [Option("public", HelpText = "Listen on all interfaces. Requires elevation or urlacl registration. By default we listen on localhost.")]
        public bool IsPublic { get; set; }
    }
}
