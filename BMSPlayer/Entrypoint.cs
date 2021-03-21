using System;
using System.Text;
using Supernova.Core;
using Luminal.Logging;
using CommandLine;
using System.Collections.Generic;

namespace Supernova
{
    class Entrypoint
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            CommandLine.Parser.Default.ParseArguments<CommandOptions>(args)
                .WithParsed(Start)
                .WithNotParsed(Exit);
        }

        static void Start(CommandOptions a)
        {
#if DEBUG
            SupernovaMain mg = new SupernovaMain(1280, 720, "Supernova (debug)", a);
#else
            SupernovaMain mg = new SupernovaMain(1280, 720, "Supernova", a);
#endif
        }

        static void Exit(IEnumerable<Error> e)
        {
            return;
        }
    }
}
