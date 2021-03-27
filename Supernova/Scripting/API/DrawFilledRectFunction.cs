using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;
using Luminal.Core;

namespace Supernova.Scripting.API
{
    [ExposeToLua("SN_DrawFilledRect")]
    class DrawFilledRectFunction
    {
        public void Execute(int x, int y, int w, int h)
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

        public Action<int, int, int, int> GetFunc => Execute;
    }
}
