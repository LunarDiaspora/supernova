using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supernova.Gameplay
{
    public static class GameplayOptions
    {
        // Special options
        public static float Offset = 0.0f; // In seconds

        // Regular options
        public static float HighSpeed = 3.0f;
        public static float UserHighSpeed = 3.0f;

        // Stupid
        public static bool Wave = false;
        public static float WavePeriod = 1.5f;
        public static float WaveScale = 2.3f;
    }
}
