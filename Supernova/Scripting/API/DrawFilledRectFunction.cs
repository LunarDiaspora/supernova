using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;
using Luminal.Core;
using Luminal.Graphics;

namespace Supernova.Scripting.API
{
    [ExposeToLua("SN_DrawFilledRect")]
    class DrawFilledRectFunction
    {
        public void Execute(int x, int y, int w, int h)
        {
            Render.Rectangle(x, y, w, h, RenderMode.FILL);
        }

        public Action<int, int, int, int> GetFunc => Execute;
    }
}
