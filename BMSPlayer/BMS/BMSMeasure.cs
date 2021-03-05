using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supernova.BMS
{
    public class BMSMeasure
    {
        public int measureNumber = 0;
        public string channel = "00"; // the channel

        public List<ChannelEvent> events = new(); // this can be notes and shit
    }
}
