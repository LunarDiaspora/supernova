using Supernova.BMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supernova.Disk
{
    public class Folder
    {
        public string Name;

        public Dictionary<string, BMSChart> Charts = new();
    }
}
