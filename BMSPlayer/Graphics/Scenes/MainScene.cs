using Supernova.Core;
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
                string.Format("{4} fps\n{0}\n{1}\n{2}\n{3}",
                    SNGlobal.Gameplay.Position, SNGlobal.Gameplay.Beat, SNGlobal.Gameplay.BPM, SNGlobal.Gameplay.bgms.Count, fps)
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

            var rect = new SDL.SDL_Rect()
            {
                x = 300,
                y = 300,
                w = 50,
                h = 50
            };

            SDL.SDL_SetRenderDrawBlendMode(Engine.Renderer, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);

            Globals.Fonts["monospace"].Draw(txt);

            SDL.SDL_SetRenderDrawColor(Engine.Renderer, 255, 0, 0, 255);
            SDL.SDL_RenderFillRect(Engine.Renderer, ref rect);
        }

        public override void OnKeyDown(Engine main)
        {
        }
    }
}
