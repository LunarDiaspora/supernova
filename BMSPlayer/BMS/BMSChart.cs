using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;
using Supernova.Gameplay;

namespace Supernova.BMS
{
    public enum PlayerType
    {
        SINGLE,
        DOUBLE
    }

    public enum ChartType
    {
        SEVENKEY,
        FIVEKEY,
        UNSUPPORTED
    }

    public enum JudgeType
    {
        VERYHARD,
        HARD,
        NORMAL,
        EASY,
        VERYEASY
    }

    public enum Difficulty
    {
        BEGINNER,
        NORMAL,
        HYPER,
        ANOTHER,
        INSANE
    }

    [MoonSharpUserData]
    public class BMSChart
    {
        //public static readonly float PULSE = 960f;

        public string title;
        public string subtitle;
        public string artist;
        public string genre;
        public int playLevel;
        public Difficulty difficulty;
        public float initialBPM = 130f;

        public PlayerType player = PlayerType.SINGLE;
        public float total = 160f;

        public Dictionary<string, BMSSample> Samples = new();
        public Dictionary<string, string> SamplePaths = new();

        public Dictionary<string, List<BMSMeasure>> Measures = new();

        public bool samplesLoaded = false;

        public TimingWindowScale rank = TimingWindowScale.VERY_EASY;

        public string MD5_Hash;
        public string SHA256_Hash;

        public BMSChart()
        {
            var channels = new[]
            {
                "00", // ?????

                "01", "02", "03", "04", // bgm, "length of #xxx", bpm change, bga base,
                "06", "07", "08", "09", // bga poor, bga layer, exbpm, stop

                "16", "11", "12", "13", "14", "15", "18", "19", // sc 1 2 3 4 5 6 7 (p1)
                "21", "22", "23", "24", "25", "28", "29", "26", // 1 2 3 4 5 6 7 sc (p2)

                "56", "51", "52", "53", "54", "55", "58", "59", // sc 1 2 3 4 5 6 7 (p1 LN)
                "61", "62", "63", "64", "65", "68", "69", "66", // 1 2 3 4 5 6 7 sc (p2 LN)

                "31", "32", "33", "34", "35", "36", "37", "38", "39", // no fucking idea
                "41", "42", "43", "44", "45", "46", "47", "48", "49", // genuinely unknown

                "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", // player 1 landmines
                "E1", "E2", "E3", "E4", "E5", "E6", "E7", "E8", "E9", // player 2 landmines
            };
            foreach (var i in channels)
            {
                Measures[i.ToLower()] = new();
            }
        }

        public List<ChannelEvent> GetAllEventsInChannel(string channel)
        {
            var t = Measures[channel];
            var l = new List<ChannelEvent>();

            foreach (var j in t)
            {
                foreach (var k in j.events)
                {
                    l.Add(k);
                }
            }
            
            return l;
        }

        public List<ChannelEvent> GetAllNotes()
        {
            var l = new List<ChannelEvent>();

            var j = new[]
            {
                "16", "11", "12", "13", "14", "15", "18", "19"
            };

            foreach (var k in j)
            {
                l.AddRange(GetAllEventsInChannel(k));
            }

            return l.OrderBy(n=>n.Beat).ToList();
        }

        public List<ChannelEvent> DebugGetAllNotableEvents()
        {
            var l = new List<ChannelEvent>();

            var j = new[]
            {
                "01", "16", "11", "12", "13", "14", "15", "18", "19"
            };

            foreach (var k in j)
            {
                l.AddRange(GetAllEventsInChannel(k));
            }

            return l.OrderBy(n => n.Beat).ToList();
        }

        public bool LoadAudioSamples()
        {
            if (samplesLoaded)
            {
                return false;
            }

            foreach (var (k, v) in SamplePaths)
            {
                var s = new BMSSample(k, v);
                Samples[k] = s;
            }

            return true;
        }
    }
}
