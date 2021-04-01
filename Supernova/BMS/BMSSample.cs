using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luminal.Audio;
using Supernova.Core;

namespace Supernova.BMS
{
    public class BMSSample
    {
        public string Channel = "00"; // 00-zz
        public GenericAudioFile File;

        public BMSSample(string Channel, string Path)
        {
            //Console.WriteLine($"Loading audio sample {Channel} from {Path}");
            File = AudioEngineManager.Engine.LoadFileFromPath(Path);
        }

        public void Play()
        {
            AudioEngineManager.Engine.PlaySound(File);
        }

        public void Destroy()
        {
            AudioEngineManager.Engine.DisposeOfFile(File);
        }
    }
}
