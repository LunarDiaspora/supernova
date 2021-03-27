using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supernova.BMS;
using Supernova.Threading;
using Supernova.Shared;
using SDL2;
using Luminal.Core;
using Supernova.Core;
using Supernova.Scripting.API;

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

        public bool Autoplay = true;

        public int Combo = 0;
        public int MaxCombo = 0;

        public int EXScore = 0;

        public static List<SDL.SDL_Scancode> Keymap = new[]
            {
                SDL.SDL_Scancode.SDL_SCANCODE_LSHIFT,
                SDL.SDL_Scancode.SDL_SCANCODE_A,
                SDL.SDL_Scancode.SDL_SCANCODE_S,
                SDL.SDL_Scancode.SDL_SCANCODE_D,
                SDL.SDL_Scancode.SDL_SCANCODE_SPACE,
                SDL.SDL_Scancode.SDL_SCANCODE_J,
                SDL.SDL_Scancode.SDL_SCANCODE_K,
                SDL.SDL_Scancode.SDL_SCANCODE_L
            }.ToList();

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

        public void LoadGameplay(BMSChart ch)
        {
            foreach (var j in Enum.GetValues(typeof(Judgement)))
            {
                JudgementCount[(Judgement)j] = 0;
            };

            _Start(ch);
        }

        void _Start(BMSChart ch)
        {
            SNGlobal.Chart = ch;

            Chart = ch;
            bgms = Chart.GetAllEventsInChannel("01");
            Notes = Chart.GetAllNotes().OrderBy(e => e.Beat).ToList();

            BPM = Chart.initialBPM;
            
            TimingWindowMap = TimingWindows.BuildTimingWindowMap(Chart.rank);
            EndOfBadWindow = TimingWindows.ScaleWindow(BaseTimingWindow.BAD, Chart.rank);

            Started = true;

            if (SNGlobal.Theme != null)
            {
                SNGlobal.Theme.OnChartLoaded();
            }

            var properTitle = ch.title;
            if (ch.subtitle != null)
            {
                properTitle = string.Format("{0} {1}", ch.title, ch.subtitle);
            }

            if (SupernovaMain.RPC != null)
            {
                SupernovaMain.RPC.SetPresence(new DiscordRPC.RichPresence()
                {
                    Details = string.Format("{0} - {1}", ch.artist, properTitle),
                    State = string.Format("{0} Lv. {1}", ch.difficulty, ch.playLevel == 0 ? "???" : ch.playLevel)
                });

            }

            SDL.SDL_SetWindowTitle(Engine.Window, string.Format("{0} | {1} - {2}", SupernovaMain.BaseTitle, ch.artist, properTitle));
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

            if (Autoplay)
            {
                var c = h.Where(t => t.Time <= Position);
                foreach (var l in c)
                {
                    HitNote(l);
                }
            }

            foreach (var n in nn)
            {
                //Chart.Samples[n.Event].Play();
                if (!Autoplay) ApplyJudgement(Judgement.POOR); // We'll just ignore that
                //else HitNote(n);
                NoteCount++;
            }

            //Notes.RemoveAll(t => t.Beat <= Beat);

            //var n = bgms.Where(t => t.Beat <= Beat);
        }

        public void JudgeKeycode(SDL.SDL_Scancode Code)
        {
            var k = Keymap.LastIndexOf(Code);
            if (k != -1) JudgeInput(k);
        }

        public void JudgeInput(int Column)
        {
            if (Autoplay) return;
            // Column = 0 through 7. Or 15. Whatever.
            var notesOnCol = Notes.Skip(NoteCount).Where(n => n.Column == Column);
            // wait hold on
            var closestNote = notesOnCol.First();
            if (closestNote != null)
            {
                HitNote(closestNote);
            }
        }

        public void HitNote(ChannelEvent closestNote)
        {
            // we have a note
            Chart.Samples[closestNote.Event].Play();
            // now let's see the timing

            var timingDelta = (Position - closestNote.Time);
            Judgement judge = Judgement.EXTRA_POOR;
            foreach (var j in TimingWindowMap)
            {
                var time = j.Item2;
                if (timingDelta <= time && timingDelta >= -time)
                {
                    judge = j.Item1;
                    //break;
                }
            }

            if (judge != Judgement.EXTRA_POOR)
            {
                NoteCount++;
                ApplyJudgement(judge);
            }
        }

        public void ApplyJudgement(Judgement j)
        {
            switch (j)
            {
                case Judgement.PERFECT_GREAT:
                    EXScore += 2;
                    Combo++;
                    break;
                case Judgement.GREAT:
                    EXScore += 1;
                    Combo++;
                    break;
                case Judgement.GOOD:
                    Combo++;
                    break;
                case Judgement.BAD:
                case Judgement.POOR:
                    Combo = 0;
                    break;
            }

            JudgementCount[j]++;
            //Console.WriteLine(j);

            if (SNGlobal.Theme != null)
            {
                SNGlobal.Theme.OnJudgement(j);
            }
        }

        public IList<ChannelEvent> GetNotes() => Notes;
    }
}
