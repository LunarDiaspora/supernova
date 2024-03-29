﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Luminal.Audio;
using System.Text.RegularExpressions;
using Supernova.Gameplay;
using System.Security.Cryptography;

namespace Supernova.BMS
{
    public class BMSParser
    {
        public static Regex channelRx = new Regex(
            @"#(\d{3})([0-9a-zA-Z]{2}):([0-9A-Za-z]{2,})", RegexOptions.Compiled
        );

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static BMSChart ParseBMSChart(string path, bool dontLoadAudio = false)
        {
            if (!File.Exists(path))
            {
                throw new ArgumentException($"There is no file at path {path}.");
            }
            var ch = new BMSChart();

            var CurrentTime = 0f;
            var CurrentBPM = 130f; // "Definition of BPM.(Beat Per Minite) at the top of music. default : 130" - Urao Yane

            var CurrentMeasure = 0;

            using (FileStream f = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                var sha = SHA256.Create();
                var shaHash = sha.ComputeHash(f);
                ch.SHA256_Hash = ByteArrayToString(shaHash).ToLower();
                f.Position = 0;
                sha.Dispose();

                var md5 = MD5.Create();
                var md5Hash = md5.ComputeHash(f);
                ch.MD5_Hash = ByteArrayToString(md5Hash).ToLower();
                f.Position = 0;
                md5.Dispose();

                using (StreamReader sr = new StreamReader(f))
                {
                    string line;
                    int lineno = -1;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lineno++;

                        string command, data;

                        var match = channelRx.Match(line);
                        if (match.Success)
                        {
                            // we have a match
                            var measure = int.Parse(match.Groups[1].Value);
                            if (measure != CurrentMeasure && measure > CurrentMeasure)
                            {
                                //Console.WriteLine($"Incrementing measure ({CurrentMeasure} -> {measure})");
                                var measureLength = (((1 / CurrentBPM) * 60) * 4); // #METER...
                                CurrentTime += measureLength;
                                CurrentMeasure = measure;
                            }
                            var channel = match.Groups[2].Value.ToLower();
                            var cdata = match.Groups[3];
                            if ((cdata.Length % 2) != 0)
                            {
                                throw new ArgumentOutOfRangeException($"BMS file malformed: channel {channel} at measure {measure} malformed (length {cdata.Length})");
                            }

                            var current = ch.Measures[channel].DefaultIfEmpty(null).FirstOrDefault(delegate (BMSMeasure t)
                            {
                                if (t == null) return false;
                                return (t.channel == channel && t.measureNumber == measure);
                            });

                            BMSMeasure meobj;
                            if (current == null)
                            {
                                meobj = new BMSMeasure
                                {
                                    channel = channel,
                                    measureNumber = measure
                                };
                            } else
                            {
                                meobj = current;
                            }

                            var evtCount = (float)(cdata.Length / 2);
                            var step = (4f / evtCount);
                            
                            for (var i=0; i<cdata.Length; i+=2)
                            {
                                var c = cdata.Value[i..(i + 2)];
                                var pulse = (step * i/2);

                                //var timeInside = ((60 / CurrentBPM) * pulse);
                                var timeInside = (((1 / CurrentBPM) * 60) * pulse);
                                var time = CurrentTime + timeInside;

                                if (c == "00") continue;

                                var evt = new ChannelEvent()
                                {
                                    BeatInMeasure = pulse,
                                    Channel = channel,
                                    Event = c,
                                    Measure = measure,
                                    Time = time
                                };

                                meobj.events.Add(evt);
                            }

                            if (meobj.events.Count == 0) continue; // what the fuck?

                            if (current == null)
                            {

                                ch.Measures[channel].Add(meobj);
                            }

                            // This is not going to be a command so
                            continue;
                        }

                        if (!ParseCommand(line, out command, out data))
                        {
                            continue;
                        }

                        if (command.StartsWith("WAV"))
                        {
                            // uh oh spaghetti-os
                            // its a wav file, better load it huh
                            string chan = command[3..];
                            string dir = Path.GetDirectoryName(path);
                            string bp = Path.Combine(dir, data);
                            //string p = Path.GetFullPath(bp);
                            string p = bp.Replace("\\", "/"); // naughty
                            ch.SamplePaths[chan] = p;
                            if (!dontLoadAudio)
                            {
                                BMSSample smp = new BMSSample(chan, p);
                                ch.Samples[chan] = smp;
                                ch.samplesLoaded = true;
                            }
                            continue;
                        }



                        switch (command.ToUpper())
                        {
                            case "PLAYER":
                                ch.player = data switch
                                {
                                    "3" => PlayerType.DOUBLE,
                                    _ => PlayerType.SINGLE
                                };
                                break;
                            case "ARTIST":
                                ch.artist = data;
                                break;
                            case "TITLE":
                                ch.title = data;
                                break;
                            case "SUBTITLE":
                                ch.subtitle = data;
                                break;
                            case "GENRE":
                                ch.genre = data;
                                break;
                            case "PLAYLEVEL":
                                try
                                {
                                    ch.playLevel = int.Parse(data);
                                } catch
                                {
                                    ch.playLevel = 0;
                                }
                                break;
                            case "DIFFICULTY":
                                try
                                {
                                    ch.difficulty = (Difficulty)int.Parse(data);
                                } catch
                                {
                                    ch.difficulty = Difficulty.INSANE;
                                }
                                break;
                            case "TOTAL":
                                ch.total = float.Parse(data);
                                break;
                            case "BPM":
                                ch.initialBPM = float.Parse(data);
                                CurrentBPM = ch.initialBPM;
                                //var ml = (((1 / CurrentBPM) * 60) * 4); // #METER...
                                //CurrentTime += ml;
                                break;
                            case "RANK":
                                ch.rank = TimingWindows.IntToWindow(int.Parse(data));
                                break;
                            default:
                                //Console.WriteLine($"Unsupported BMS command {command}");
                                break;
                        }

                        continue;
                    }
                }
            }

            /*
                // temporary
                var audio = ch.Samples["01"];
                audio.Play();
                ch.Samples["03"].Play(); // oh boy
            */

            return ch;
        }

        public static bool ParseCommand(string inp, out string cmd, out string par)
        {
            cmd = null;
            par = null;

            int start = 0;
            
            while (true)
            {
                if (start >= inp.Length) return false;
                if (inp[start] == '#') break;
                start++;
            }

            var t = inp.Substring(start + 1);

            int commandSplit = 0;
            while (true)
            {
                if (commandSplit >= t.Length) return false;
                if (t[commandSplit] == ' ') break;
                commandSplit++;
            }

            cmd = t.Substring(0, commandSplit);
            par = t[(commandSplit + 1)..];

            return true;
        }
    }
}
