using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;
using Luminal.Core;
using MoonSharp.Interpreter;

namespace Supernova.Scripting.API
{
    [ExposeToLua("SN_SetDrawColour")]
    class SetDrawColourFunction : LuaFunction
    {
        public override DynValue Execute(params DynValue[] args)
        {
            byte r = (byte)args[0].Number;
            byte g = (byte)args[1].Number;
            byte b = (byte)args[2].Number;
            byte a = 255;
            if (args.Length == 4)
            {
                a = (byte)args[3].Number;
            }

            SDL.SDL_SetRenderDrawColor(Engine.Renderer, r, g, b, a);
            Context.SetColour(r, g, b, a);
            return null;
        }
    }
}
