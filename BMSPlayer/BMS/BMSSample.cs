using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using Luminal.Audio;
using Supernova.Core;

namespace Supernova.BMS
{
    class BMSSample
    {
        public string Channel = "00"; // 00-zz
        public UniversalAudioFile File;

        public BMSSample(string Channel, string Path)
        {
            Console.WriteLine($"Loading audio sample {Channel} from {Path}");
            File = new UniversalAudioFile(Path);
            if (File.Stream == null)
            {
                Console.WriteLine("Loading failure!");
            } else
            {
                Console.WriteLine("Loaded successfully");
            }
        }

        public void Play()
        {
            AudioEngine.Instance.PlaySound(File);
        }

        public static explicit operator UniversalAudioFile(BMSSample x)
        {
            return x.File;
        }
    }
}
