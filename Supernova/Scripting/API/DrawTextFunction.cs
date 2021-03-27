using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luminal.Core;

namespace Supernova.Scripting.API
{
    [ExposeToLua("SN_DrawText")]
    class DrawTextFunction
    {
        public void Execute(string text, int x, int y)
        {
            Globals.Fonts[Context.CurrentFont].Draw(text, x, y);
        }

        public Action<string, int, int> GetFunc => Execute;
    }
}
