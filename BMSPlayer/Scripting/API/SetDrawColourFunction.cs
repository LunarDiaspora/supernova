using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;
using Luminal.Core;

namespace Supernova.Scripting.API
{
    [ExposeToLua("SN_SetDrawColour")]
    class SetDrawColourFunction
    {
        public void Execute(byte r, byte g, byte b, byte a = 255)
        {
            SDL.SDL_SetRenderDrawColor(Engine.Renderer, r, g, b, a);
            Context.SetColour(r, g, b, a);
        }

        public Action<byte, byte, byte, byte> GetFunc => Execute;
    }
}
