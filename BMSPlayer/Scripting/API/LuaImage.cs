using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luminal.Core;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

namespace Supernova.Scripting.API
{
    [MoonSharpUserData]
    public class LuaImage
    {
        [MoonSharpVisible(false)]
        public Image img;

        [MoonSharpVisible(false)]
        public LuaImage(Image source)
        {
            img = source;
        }

        [MoonSharpVisible(true)]
        public void Draw(int x, int y)
        {
            img.Draw(x, y);
        }
    }
}
