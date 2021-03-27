using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace Supernova.Scripting
{
    class SnthemeReader
    {
        public static SNThemeFile ReadSntheme(string Path)
        {
            var j = File.ReadAllText(Path);
            var k = JsonSerializer.Deserialize<SNThemeFile>(j);
            return k;
        }
    }
}
