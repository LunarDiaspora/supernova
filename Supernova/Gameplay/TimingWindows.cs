using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supernova.Gameplay
{
    public enum BaseTimingWindow
    {
        // LR2 base-timings, but percent-based scaling
        // This describes "EASY"
        PERFECT_GREAT = 21,
        GREAT = 60,
        GOOD = 120,
        BAD = 200
    }

    public enum TimingWindowScale
    {
        VERY_HARD = 50,
        HARD = 65,
        NORMAL = 80,
        EASY = 100,
        VERY_EASY = 130
    }

    class TimingWindows
    {
        public static TimingWindowScale IntToWindow(int a)
        {
            var j = new[]
            {
                TimingWindowScale.VERY_HARD,
                TimingWindowScale.HARD,
                TimingWindowScale.NORMAL,
                TimingWindowScale.EASY,
                TimingWindowScale.VERY_EASY
            };
            return j[a];
        }

        public static BaseTimingWindow JudgementToBaseTimingWindow(Judgement j)
        {
            var map = new Dictionary<Judgement, BaseTimingWindow>
            {
                { Judgement.PERFECT_GREAT, BaseTimingWindow.PERFECT_GREAT },
                { Judgement.GREAT, BaseTimingWindow.GREAT },
                { Judgement.GOOD, BaseTimingWindow.GOOD },
                { Judgement.BAD, BaseTimingWindow.BAD }
            };

            return map[j];
        }

        public static List<(Judgement, float)> BuildTimingWindowMap(TimingWindowScale scale)
        {
            var map = new List<(Judgement, float)>();

            foreach (var e in new[]
            {
                Judgement.BAD,
                Judgement.GOOD,
                Judgement.GREAT,
                Judgement.PERFECT_GREAT
            })
            {
                var t = JudgementToBaseTimingWindow(e);
                var scaled_tw = ScaleWindow(t, scale);
                map.Add((e, scaled_tw));
            }

            return map;
        }

        public static float ScaleWindow(BaseTimingWindow w, TimingWindowScale scale)
        {
            var tw = (int)w;
            var scaled_tw = tw * ((float)scale / 100f);
            return scaled_tw/1000f;
        }
    }
}
