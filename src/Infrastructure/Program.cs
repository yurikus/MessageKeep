using System;
using CommandLine;

namespace MessageKeep
{
    class Program
    {
        static void Main(string[] args_)
        {
            var parser = new Parser(cnf =>
            {
                cnf.EnableDashDash = true;
                cnf.HelpWriter = Console.Out;
            });

            parser.ParseArguments<CmdLineOptions>(args_)
                .WithParsed(opts => Run(parser, opts));
        }

        static void Run(Parser parser_, CmdLineOptions opts_)
        {
            Console.WriteLine("Press Ctrl-Q to exit.");
            Console.WriteLine("Running with args: " + parser_.FormatCommandLine(opts_));

            if (opts_.Port <= ushort.MinValue || opts_.Port > ushort.MaxValue)
            {
                Console.WriteLine("ERROR: Invalid value for --port");
                return;
            }

            var app = new AppCore();
            app.Start(opts_);

            while (true)
            {
                var cki = Console.ReadKey(true);

                bool mod_Ctrl = (cki.Modifiers & ConsoleModifiers.Control) != 0;
                bool key_Q = cki.Key == ConsoleKey.Q;

                if (mod_Ctrl && key_Q)
                    break;
            }

            app.Stop();
        }
    }
}
