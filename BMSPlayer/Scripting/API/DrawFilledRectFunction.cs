using MoonSharp.Interpreter;
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
    class DrawFilledRectFunction : LuaFunction
    {
        public override DynValue Execute(params DynValue[] args)
        {
            int x = (int)Math.Floor(args[0].Number);
            int y = (int)Math.Floor(args[1].Number);
            int w = (int)Math.Floor(args[2].Number);
            int h = (int)Math.Floor(args[3].Number);

            var r = new SDL.SDL_Rect()
            {
                x = x,
                y = y,
                w = w,
                h = h
            };

            SDL.SDL_RenderFillRect(Engine.Renderer, ref r);

            return null;
        }
    }
}
