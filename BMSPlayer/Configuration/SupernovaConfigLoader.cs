using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace Supernova.Configuration
{
    class SupernovaConfigLoader
    {
        public static SupernovaConfig LoadConfig(string Path)
        {
            var f = File.ReadAllText(Path);
            var js = JsonSerializer.Deserialize<SupernovaConfig>(f);
            return js;
        }
    }
}
