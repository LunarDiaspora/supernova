using Luminal.Core;
using Supernova.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neo.IronLua;

namespace Supernova.Scripting.API
{
    [ExposeToLua("SN_GetTextDimensions")]
    class GetTextDimensions
    {
        public LuaTable Execute(string text)
        {
            dynamic j = new LuaTable();

            Globals.Fonts[Context.CurrentFont].GetDimensions(text ?? "", out int w, out int h);

            j.w = w;
            j.h = h;

            return j;
        }

        public Func<string, LuaTable> GetFunc => Execute;
    }
}
