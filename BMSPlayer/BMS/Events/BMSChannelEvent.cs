using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supernova.BMS.Events
{
    class BMSChannelEvent
    {
        public string Id = ""; // 00 - ZZ
        public float Pulse = 0f;

        public virtual void OnHit()
        {

        }

    }
}
