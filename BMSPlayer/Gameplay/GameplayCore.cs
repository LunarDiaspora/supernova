using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supernova.BMS;
using Supernova.Threading;
using Supernova.Shared;
using MoonSharp.Interpreter;

namespace Supernova.Gameplay
{
    [MoonSharpUserData]
    public class GameplayCore
    {
        public BMSChart Chart;
        public float BPM;
        public float Beat = 0f;
        public float Position = 0f;

        public bool Started = false;

        public List<ChannelEvent> bgms;
        public List<ChannelEvent> Notes;

        public int BgmCount = 0;
        public int NoteCount = 0;
        public int BgmFrameCount = 0;

        public void LoadGameplay(string path)
        {
            var cw = new ChartLoadingWorker();
            cw.OnChartDoneLoading += _Start;
            cw.StartLoadingChart(path);
        }

        void _Start(BMSChart ch)
        {
            // Hey why is there a race condition here I don't understand

            Console.WriteLine("_Start");

            SNGlobal.Chart = ch;

            Chart = ch;
            bgms = Chart.GetAllEventsInChannel("01");
            Notes = Chart.GetAllNotes();

            BPM = Chart.initialBPM;

            Started = true;

            if (SNGlobal.Theme != null)
            {
                SNGlobal.Theme.OnChartLoaded();
            }
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
            var nn = h.Where(t => t.Beat <= Beat);

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
                Chart.Samples[n.Event].Play();
                NoteCount++;
            }

            //Notes.RemoveAll(t => t.Beat <= Beat);

            //var n = bgms.Where(t => t.Beat <= Beat);
        }

        public IList<ChannelEvent> GetNotes() => Notes;
    }
}
