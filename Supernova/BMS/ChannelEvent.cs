using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supernova.Gameplay;
using Supernova.Shared;

namespace Supernova.BMS
{
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
        //public float Time = 0f;

        public float Measure = 0;
        public string Event = "00";
        public string Channel = "00";

        public int? Column
        {
            get
            {
                var e = new string[]
                {
                    "16", "11", "12", "13", "14", "15", "18", "19"
                };
                int? j = Array.IndexOf(e, Channel);
                if (j == -1) j = null;
                return j;
            }
        }

        private float _Time = 0f;
        public float Time
        {
            get
            {
                return _Time + GameplayOptions.Offset;
            }
            set
            {
                _Time = value;
            }
        }

        //public float Time
        //{
        //    get
        //    {
        //        return Beat * (60 / SNGlobal.Gameplay.GetBPMAtBeat(Beat));
        //    }
        //}
    }
}
