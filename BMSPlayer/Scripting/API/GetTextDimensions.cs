using Luminal.Core;
using MoonSharp.Interpreter;
using Supernova.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supernova.Scripting.API
{
    [ExposeToLua("SN_GetTextDimensions")]
    class GetTextDimensions : LuaFunction
    {
        public override DynValue Execute(params DynValue[] args)
        {
            var j = new Table(SNGlobal.Theme.script);
            int w;
            int h;

            Globals.Fonts[Context.CurrentFont].GetDimensions(args[0].String, out w, out h);

            j.Set("w", DynValue.NewNumber(w));
            j.Set("h", DynValue.NewNumber(h));

            return DynValue.NewTable(j);
        }
    }
}
