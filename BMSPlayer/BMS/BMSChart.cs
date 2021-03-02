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
    }
}
