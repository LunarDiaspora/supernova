using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luminal.Core;
using Luminal.Graphics;
using Luminal.Logging;
using Luminal.OpenGL;
using SDL2;
using Supernova.Shared;

namespace Supernova.Graphics.Scenes
{
    [SceneDefinition("Startup")]
    class Startup : Scene
    {
        private static DateTime date;

        public override void OnEnter()
        {
            date = DateTime.Now;
        }

        public override void Draw(Engine _)
        {
            var str = new StringBuilder();
            str.Append("Supernova (client)\n");
            str.Append($"Starting on {date.Year}/{date.Month}/{date.Day} {date.Hour}:{date.Minute}:{date.Second}\n");
            str.Append($"OpenGL version {OpenGLManager.Version.VersionString}\n");
            str.Append($"\n\nPress any key to continue.");

            Globals.Fonts["monospace"].Draw(str.ToString(), 10, 10);
        }

        public override void OnKeyDown(Engine main, SDL.SDL_Scancode sc)
        {
            SNGlobal.SwitchTheme("SongSelect");
            Log.Debug($"Lua: Song-select theme name is '{SNGlobal.Theme.Name}'.");

            main.sceneManager.SwitchScene("Main");
        }
    }
}
