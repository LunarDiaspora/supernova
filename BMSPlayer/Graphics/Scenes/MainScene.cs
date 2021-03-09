﻿using Supernova.Core;
using Luminal.Core;
using Luminal.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;
using Supernova.Shared;
using SDL2;

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
                string.Format("{4} fps\n{0}\n{1}\n{2}\n{3} - {5}",
                    SNGlobal.Gameplay.Position, SNGlobal.Gameplay.Beat, SNGlobal.Gameplay.BPM, SNGlobal.Gameplay.bgms.Count, fps,
                    SNGlobal.Gameplay.Notes.Count)
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

            Context.SetColour(255, 255, 255, 255);
            Globals.Fonts["monospace"].Draw(txt);
        }

        public override void OnKeyDown(Engine main)
        {
        }
    }
}
