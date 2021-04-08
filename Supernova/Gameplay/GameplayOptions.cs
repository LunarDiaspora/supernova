using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Supernova.Gameplay
{
    [Serializable]
    public class GameplayOptions
    {
        // Special options
        public float Offset = 0.0f; // In seconds

        // Regular options
        [JsonIgnore]
        public float HighSpeed = 3.0f;

        public float UserHighSpeed = 3.0f;

        // Stupid
        public bool Wave = false;
        public float WavePeriod = 1.5f;
        public float WaveScale = 2.3f;

        [NonSerialized]
        public static GameplayOptions Instance = new();
    }
}
