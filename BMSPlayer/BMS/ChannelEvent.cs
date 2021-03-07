using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;

namespace Supernova.BMS
{
    [MoonSharpUserData]
    public class ChannelEvent
    {
        public float BeatInMeasure = 0f;
        public float Beat
        {
            get
            {
                return BeatInMeasure + (Measure * 4);
                // Assume measures are 4 wide unless proven otherwise.
                // We'll worry about #METER... later. Not now.
            }
        }
        public float Measure = 0;
        public string Event = "00";
        public string Channel = "00";
    }
}
