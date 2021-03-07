using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luminal.Core;
using MoonSharp.Interpreter;

namespace Supernova.Scripting.API
{
    [ExposeToLua("SN_SetFont")]
    class SetFontFunction : LuaFunction
    {
        public override DynValue Execute(params DynValue[] args)
        {
            var newFont = args[0].String;
            if (Globals.Fonts.ContainsKey(newFont))
            {
                Context.CurrentFont = newFont;
                return DynValue.NewBoolean(true);
            }
            return DynValue.NewBoolean(false);
        }
    }
}
