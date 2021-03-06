﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luminal.Audio;
using NAudio.Wave;
using NAudio.Vorbis;
using NAudio.FileFormats.Mp3;
using System.IO;

namespace LuminalNAudioEngine
{
    public class NAudioFile : GenericAudioFile
    {
        public RewindingStream Stream;

        public NAudioFile(string Path)
        {
            Begin(Path);
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

            if (extension == ".wav")
            {
                // wav file
                var st = new WaveFileReader(path);
                Stream = new RewindingStream(st);
                return true;
            }
            else if (extension == ".ogg")
            {
                var st = new VorbisWaveReader(path);
                Stream = new RewindingStream(st);
                return true;
            }
            else if (extension == ".mp3")
            {
                // Oh shit, mp3 file
                // it's big boy time
                var st = new MediaFoundationReader(path); // ?? 
                Stream = new RewindingStream(st);
                return true;
            }
            else
            {
                Console.WriteLine($"Unknown file type {extension}.");
                return false;
            }
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
