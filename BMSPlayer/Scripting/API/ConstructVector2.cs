using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;
using SFML.System;

namespace Supernova.Scripting.API
{
    [ExposeToLua("Vector2")]
    class ConstructVector2 : LuaFunction
    {
        public override DynValue Execute(params DynValue[] args)
        {
            var a = (float)args[0].Number;
            var b = (float)args[1].Number;
            var v = new Vector2f(a, b);
            return UserData.Create(v);
        }
    }
}
