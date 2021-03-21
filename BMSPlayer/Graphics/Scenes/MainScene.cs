using Supernova.Core;
using Luminal.Core;
using Luminal.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;
using Supernova.Shared;
using SDL2;
using Supernova.Scripting;

namespace Supernova.Graphics.Scenes
{
    [SceneDefinition("Main")]
    class MainScene : Scene
    {
        float dt = 0.0f;
        float fps = 0.0f;
        string txt = "";

        public MainScene()
        {
            
        }

        public override void Update(Engine main, float deltaTime)
        {
            dt += deltaTime;
            dt /= 2.0f;
            fps = 1.0f / dt;

            txt = SNGlobal.Gameplay.Started ?
                string.Format("{4} fps\n{0}\n{1}\n{2}\n{3} - {5}\n{6}",
                    SNGlobal.Gameplay.Position, SNGlobal.Gameplay.Beat, SNGlobal.Gameplay.BPM, SNGlobal.Gameplay.bgms.Count, fps,
                    SNGlobal.Gameplay.Notes.Count, SNGlobal.Gameplay.NoteCount)
                : "LOADING CHART...";
            //t.DisplayedString = string.Format("{0} fps\n{1}", Math.Floor(fps), oa);
        }

        public void FillRect(int x, int y, int w, int h)
        {
            var r = new SDL.SDL_Rect()
            {
                x = x,
                y = y,
                w = w,
                h = h
            };

            SDL.SDL_RenderFillRect(Engine.Renderer, ref r);
        }

        public void SetColour(Colour c)
        {
            SDL.SDL_SetRenderDrawColor(Engine.Renderer, (byte)c.r, (byte)c.g, (byte)c.b, (byte)c.a);
        }

        public override void Draw(Engine main)
        {
            /*foreach (var i in Drawables)
            {
                main.Window.Draw(i);
            }

            main.Window.Draw(t);*/

            SDL.SDL_SetRenderDrawBlendMode(Engine.Renderer, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);

            if (SupernovaMain.State == SupernovaState.PLAY)
            {
                // Draw notes
                if (SNGlobal.Gameplay.Started)
                {
                    for (int i = SNGlobal.Gameplay.NoteCount; i < SNGlobal.Gameplay.Notes.Count; i++)
                    {
                        var note = SNGlobal.Gameplay.Notes[i];
                        float Stop = (Engine.Height - SNGlobal.Theme.NoteYOffset);
                        float CalculatedY = Stop -
                                            ((note.Time - SNGlobal.Gameplay.Position) * SNGlobal.Theme.NoteHeight * (36));
                        int ActualY = Math.Min((int)CalculatedY, (int)Stop);
                        if (CalculatedY > -SNGlobal.Theme.NoteHeight)
                        {
                            var col = SNGlobal.Theme.ChartColours[(int)note.Column];
                            SetColour(col);
                            var X = SNGlobal.Theme.NoteXOffset + (SNGlobal.Theme.NoteWidth * note.Column);
                            FillRect((int)X, ActualY, SNGlobal.Theme.NoteWidth, SNGlobal.Theme.NoteHeight);

                            //Globals.Fonts["monospace"].Draw($"{note.Time}", (int)X, ActualY);
                        }
                    }
                }

                Context.SetColour(255, 255, 255, 255);
                Globals.Fonts["monospace"].Draw(txt);

                if (SNGlobal.Theme != null)
                {
                    SNGlobal.Theme.DrawAfterNotes();
                }
            }
        }

        public override void OnKeyDown(Engine main, SDL.SDL_Scancode sc)
        {
            if (SNGlobal.Gameplay.Started && SupernovaMain.State == SupernovaState.PLAY)
            {
                SNGlobal.Gameplay.JudgeKeycode(sc);
            }
        }
    }
}
