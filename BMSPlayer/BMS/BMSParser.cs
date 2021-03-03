using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Luminal.Audio;
using System.Text.RegularExpressions;

namespace Supernova.BMS
{
    class BMSParser
    {
        public static Regex channelRx = new Regex(
            @"#(\d{3})([0-9a-zA-Z]{2}):([0-9A-Za-z]{2,})", RegexOptions.Compiled
        );

        public static BMSChart ParseBMSChart(string path)
        {
            if (!File.Exists(path))
            {
                throw new ArgumentException($"There is no file at path {path}.");
            }
            var ch = new BMSChart();

            using (FileStream f = File.Open(path, FileMode.Open, FileAccess.Read))
            {
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
                            var channel = match.Groups[2].Value.ToLower();
                            var cdata = match.Groups[3];
                            if ((cdata.Length % 2) != 0)
                            {
                                throw new ArgumentOutOfRangeException($"BMS file malformed: channel {channel} at measure {measure} malformed (length {cdata.Length})");
                            }

                            var meobj = new BMSMeasure
                            {
                                channel = channel,
                                measureNumber = measure
                            };

                            var evtCount = (cdata.Length / 2);
                            var step = (BMSChart.PULSE / evtCount);
                            
                            for (var i=0; i<cdata.Length; i+=2)
                            {
                                var c = cdata.Value[i..(i + 2)];
                                var pulse = (step * (i/2));

                                if (c == "00") continue;

                                var evt = new ChannelEvent()
                                {
                                    Pulse = pulse,
                                    Channel = channel,
                                    Event = c,
                                    Measure = measure
                                };

                                meobj.events.Add(evt);
                            }

                            ch.Measures[channel].Add(meobj);

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
                            //Console.WriteLine($"{chan} : {p}");

                            BMSSample smp = new BMSSample(chan, p);
                            ch.Samples.Add(chan, smp);

                            continue;
                        }

                        

                        switch (command)
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
                                ch.playLevel = int.Parse(data);
                                break;
                            case "DIFFICULTY":
                                ch.difficulty = (Difficulty)int.Parse(data);
                                break;
                            case "TOTAL":
                                ch.total = float.Parse(data);
                                break;
                            default:
                                Console.WriteLine($"Unsupported BMS command {command}");
                                break;
                        }

                        continue;
                    }
                }
            }

            // temporary
            var audio = ch.Samples["01"];
            audio.Play();
            ch.Samples["03"].Play(); // oh boy

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
