using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NAudio.FileFormats;

namespace Luminal.Audio
{
    public class AudioEngine : IDisposable
    {
        public IWavePlayer SoundCard;
        public MixingSampleProvider Mixer;
        public WaveFormat wf;

        public AudioEngine(int sampleRate = 44100, int channels = 2)
        {
            wf = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channels);
        }

        public void Initialise()
        {
            Console.WriteLine($"Initialising Luminal AudioEngine @ {wf.SampleRate} Hz, with {wf.Channels} channels");
            SoundCard = new WaveOutEvent();
            Mixer = new MixingSampleProvider(wf);
            Mixer.ReadFully = true;
            SoundCard.Init(Mixer);
            SoundCard.Play();
        }

        public void Dispose()
        {
            Console.WriteLine("AudioEngine disposing...");
            SoundCard.Dispose();
        }

        public void PlaySound(UniversalAudioFile File)
        {
            if (File.Stream.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
            {
                // Remux to the good one so NAudio doesnt scream
                var ns = WaveFormatConversionStream.CreatePcmStream(File.Stream);
                InternalAddMixerInput(ns.ToSampleProvider());
            } else
            {
                InternalAddMixerInput(File.Stream.ToSampleProvider());
            }
        }

        private void InternalAddMixerInput(ISampleProvider Input)
        {
            Mixer.AddMixerInput(ConvertChannels(Input));
        }

        private ISampleProvider ConvertChannels(ISampleProvider Input)
        {
            if (Input.WaveFormat.Channels == Mixer.WaveFormat.Channels)
            {
                return Input;
            }
            if (Input.WaveFormat.Channels == 1 && Mixer.WaveFormat.Channels == 2)
            {
                return new MonoToStereoSampleProvider(Input);
            }
            throw new NotImplementedException($"Channel count mismatch! (Mixer wants {Mixer.WaveFormat.Channels} yet audio source is {Input.WaveFormat.Channels})");
        }

        public static readonly AudioEngine Instance = new AudioEngine(44100, 2);
    }
}
