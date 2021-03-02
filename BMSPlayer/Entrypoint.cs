using System;
using System.Text;
using Supernova.Core;

namespace Supernova
{
    class Entrypoint
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

#if DEBUG
            SupernovaMain mg = new SupernovaMain(1280, 720, "Supernova (debug)");
#else
            SupernovaMain mg = new SupernovaMain(1280, 720, "Supernova");
#endif
        }
    }
}
