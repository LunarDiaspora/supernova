using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    class BMSChart
    {
        public static readonly float PULSE = 960f;

        public string title;
        public string subtitle;
        public string artist;
        public string genre;
        public int playLevel;
        public Difficulty difficulty;
        public float initialBPM;

        public PlayerType player = PlayerType.SINGLE;
        public float total = 160f;

        public Dictionary<string, BMSSample> Samples = new();

        public Dictionary<string, List<BMSMeasure>> Measures = new();

        public BMSChart()
        {
            var channels = new[]
            {
                "00", // ?????

                "01", "02", "03", "04", // bgm, "length of #xxx", bpm change, bga base
                "06", "07", "08",       // bga poor, bga layer, exbpm

                "16", "11", "12", "13", "14", "15", "18", "19", // sc 1 2 3 4 5 6 7 (p1)
                "21", "22", "23", "24", "25", "27", "28", "26", // 1 2 3 4 5 6 7 sc (p2)
            };
            foreach (var i in channels)
            {
                Measures[i.ToLower()] = new();
            }
        }
    }
}
