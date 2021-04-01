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
using Supernova.Gameplay;

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
                                            ((note.Time - SNGlobal.Gameplay.Position) * SNGlobal.Theme.NoteHeight * (12 * GameplayOptions.HighSpeed));
                        int ActualY = Math.Min((int)CalculatedY, (int)Stop);
                        if (CalculatedY > -SNGlobal.Theme.NoteHeight)
                        {
                            var col = SNGlobal.Theme.ChartColours[(int)note.Column];
                            Context.SetColour((byte)col.r, (byte)col.g, (byte)col.b, (byte)col.a);
                            var X = SNGlobal.Theme.NoteXOffset + (SNGlobal.Theme.NoteWidth * note.Column);
                            Render.Rectangle((int)X, ActualY, SNGlobal.Theme.NoteWidth, SNGlobal.Theme.NoteHeight, RenderMode.FILL);

                            //Globals.Fonts["monospace"].Draw($"{note.Time}", (int)X, ActualY);
                        }
                    }
                }

                Context.SetColour(255, 255, 255, 255);
                Globals.Fonts["monospace"].Draw(txt, 0f, 0f);

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
