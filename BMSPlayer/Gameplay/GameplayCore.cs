using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supernova.BMS;
using Supernova.Threading;
using Supernova.Shared;
using MoonSharp.Interpreter;
using SDL2;
using Luminal.Core;
using Supernova.Core;

namespace Supernova.Gameplay
{
    public enum Judgement
    {
        PERFECT_GREAT,
        GREAT,
        GOOD,
        BAD,
        POOR,
        EXTRA_POOR = -1
    }

    [MoonSharpUserData]
    public class GameplayCore
    {
        public BMSChart Chart;
        public float BPM;
        public float Beat = 0f;
        public float Position = 0f;

        public float CurrentBPMStartBeat = 0f;
        public float CurrentBPMStartTime = 0f;

        public bool Started = false;

        public List<ChannelEvent> bgms;
        public List<ChannelEvent> Notes;

        public List<(Judgement, float)> TimingWindowMap;
        public float EndOfBadWindow;

        public Dictionary<Judgement, int> JudgementCount = new();

        public int BgmCount = 0;
        public int NoteCount = 0;
        public int BgmFrameCount = 0;

        public void LoadGameplay(string path)
        {
            var cw = new ChartLoadingWorker();
            cw.OnChartDoneLoading += _Start;
            cw.StartLoadingChart(path);

            foreach (var j in Enum.GetValues(typeof(Judgement)))
            {
                JudgementCount[(Judgement)j] = 0;
            };
        }

        void _Start(BMSChart ch)
        {
            SNGlobal.Chart = ch;

            Chart = ch;
            bgms = Chart.GetAllEventsInChannel("01");
            Notes = Chart.GetAllNotes();

            BPM = Chart.initialBPM;
            
            TimingWindowMap = TimingWindows.BuildTimingWindowMap(Chart.rank);
            EndOfBadWindow = TimingWindows.ScaleWindow(BaseTimingWindow.BAD, Chart.rank);

            Started = true;

            if (SNGlobal.Theme != null)
            {
                SNGlobal.Theme.OnChartLoaded();
            }

            SDL.SDL_SetWindowTitle(Engine.Window, string.Format("{0} | {1} - {2}", SupernovaMain.BaseTitle, ch.artist, ch.title));
        }

        public void UpdateEngine(float Delta)
        {
            if (!Started) return;
            Position += Delta;

            var BPS = BPM / 60;
            var DeltaBeat = BPS * Delta;
            Beat += DeltaBeat;

            //Notes.RemoveAll(t => t.Beat <= Beat);

            var h = Notes.Skip(NoteCount);

            //var BgmFrameAmount = 50;

            var nb = bgms.Where(t => t.Beat <= Beat);
            var nn = h.Where(t => t.Time+EndOfBadWindow <= (Position));

            //var evts = bgms.Where(t => t.Beat <= Beat); // BGMs that have passed
            foreach (var f in nb)
            {
                //Console.WriteLine($"{f.Event} {f.Beat} {f.Channel}");
                Chart.Samples[f.Event].Play();
                //bgms.Remove(f);
                BgmCount++;
                //if (BgmCount >= BgmFrameAmount)
                //{
                //    BgmCount = 0;
                //    BgmFrameCount++;
                //}
            }

            bgms.RemoveAll(t => t.Beat <= Beat);

            foreach (var n in nn)
            {
                //Chart.Samples[n.Event].Play();
                JudgementCount[Judgement.POOR]++;
                NoteCount++;
            }

            //Notes.RemoveAll(t => t.Beat <= Beat);

            //var n = bgms.Where(t => t.Beat <= Beat);
        }

        public float GetBPMAtBeat(float x)
        {
            return BPM; // FUCKING TEMP AS SHIT
        }

        public IList<ChannelEvent> GetNotes() => Notes;
    }
}
