using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using Supernova.Graphics.Scenes;

namespace Supernova.Scripting.API
{
    [ExposeToLua("SN_AddRectangle")]
    class AddBoxFunction : LuaFunction
    {
        public override DynValue Execute(params DynValue[] args)
        {
            var vec2 = new Vector2f((float)args[0].Number, (float)args[1].Number);
            var n = new RectangleShape(vec2);
            MainScene.Drawables.Add(n);
            var ud = UserData.Create(n);
            return ud;
        }
    }
}
