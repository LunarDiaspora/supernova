using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagedBass;

namespace Luminal.Audio
{
    public class AudioEngine : IDisposable
    {
        int sampleRate;


        public AudioEngine(int sr = 44100)
        {
            sampleRate = sr;
        }

        public void Initialise()
        {
            Console.WriteLine($"Initialising Luminal AudioEngine @ {sampleRate} Hz");
            Bass.Init(-1, sampleRate);
        }

        public void Dispose()
        {
            Console.WriteLine("AudioEngine disposing...");
            Bass.Free();
        }

        public void PlaySound(UniversalAudioFile File)
        {
            Bass.ChannelPlay((int) File.Stream);
        }

        public static readonly AudioEngine Instance = new AudioEngine(44100);
    }
}
