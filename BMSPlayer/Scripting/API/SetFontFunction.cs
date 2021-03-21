using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luminal.Core;

namespace Supernova.Scripting.API
{
    [ExposeToLua("SN_SetFont")]
    class SetFontFunction
    {
        public bool Execute(string newFont)
        {
            if (Globals.Fonts.ContainsKey(newFont))
            {
                Context.CurrentFont = newFont;
                return true;
            }
            return false;
        }

        public Func<string, bool> GetFunc => Execute;
    }
}
