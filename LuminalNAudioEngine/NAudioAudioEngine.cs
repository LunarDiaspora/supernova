using System;
using Luminal.Audio;
using Luminal.Modular;
using NAudio;
using NAudio.Wave;
using NAudio.Vorbis;
using NAudio.Wave.SampleProviders;
using NAudio.FileFormats;

namespace LuminalNAudioEngine
{
    [LuminalAudioModule()]
    public class NAudioAudioEngine : GenericAudioEngine
    {
        public IWavePlayer SoundCard;
        public MixingSampleProvider Mixer;
        public WaveFormat wf;

        public NAudioAudioEngine(int sr = 44100) : base(sr)
        {
            wf = WaveFormat.CreateIeeeFloatWaveFormat(sr, 2);
        }

        public override void Initialise()
        {
            Console.WriteLine($"Luminal NAudio Module init @ {wf.SampleRate} Hz, with {wf.Channels} channels");
            Console.WriteLine("YOU SHOULD NOT BE USING NAUDIO. LuminalFMODCoreEngine is the way to go now.");
            SoundCard = new WaveOutEvent();
            Mixer = new MixingSampleProvider(wf);
            Mixer.ReadFully = true;
            SoundCard.Init(Mixer);
            SoundCard.Play();
        }

        public override void Dispose()
        {
            Console.WriteLine("Luminal NAudio Module dispose");
            SoundCard.Dispose();
        }

        public override NAudioFile LoadFileFromPath(string p)
        {
            var f = new NAudioFile(p);
            return f;
        }

        public override void PlaySound(GenericAudioFile f)
        {
            var File = (NAudioFile)f;
            if (File.Stream.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
            {
                // Remux to the good one so NAudio doesnt scream
                var ns = WaveFormatConversionStream.CreatePcmStream(File.Stream);
                InternalAddMixerInput(ns.ToSampleProvider());
            }
            else
            {
                File.Stream.PlaybackEnd += delegate
                {
                    Mixer.RemoveMixerInput(ConvertChannels(File.Stream.ToSampleProvider()));
                };
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
    }
}
