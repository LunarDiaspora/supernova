using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ManagedBass;

namespace Luminal.Audio
{
    public class UniversalAudioFile
    {
        public int? Stream;

        public UniversalAudioFile(string path)
        {
            Begin(path);
        }

        bool Begin(string path)
        {
            var extension = Path.GetExtension(path);

            if (!File.Exists(path))
            {
                // The file literally isn't there. Typo in BMS?
                var p = TryAlternativeFmt(extension);
                if (Begin(Path.ChangeExtension(path, p)))
                {
                    //Console.WriteLine($"Sigh... It said {extension}, but it's actually a {p}. Great. Oh well, we did it.");
                    return true;
                }
                return false;
            }

            Stream = Bass.CreateStream(path, 0, 0, BassFlags.Default);
            if (Stream == 0) Stream = null;
            // stuff here
            return true;
        }

        string TryAlternativeFmt(string t)
        {
            var h = ".ogg";

            switch (t)
            {
                case ".wav":
                    h = ".ogg";
                    break;
                case ".ogg":
                    h = ".mp3";
                    break;
                case ".mp3":
                    h = ".wav";
                    break;
            }

            return h;
        }
    }
}
