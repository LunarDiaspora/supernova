using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luminal.Core;
using MoonSharp.Interpreter;

namespace Supernova.Scripting.API
{
    [ExposeToLua("SN_DrawText")]
    class DrawTextFunction : LuaFunction
    {
        public override DynValue Execute(params DynValue[] args)
        {
            var x = (int)args[1].Number;
            var y = (int)args[2].Number;
            var text = args[0].String;
            Globals.Fonts[Context.CurrentFont].Draw(text, x, y);
            return null;
        }
    }
}
