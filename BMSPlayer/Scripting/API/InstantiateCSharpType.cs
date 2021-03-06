using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;

namespace Supernova.Scripting.API
{
    [ExposeToLua("SN_CreateCSharpType")]
    class InstantiateCSharpType : LuaFunction
    {
        public override DynValue Execute(params DynValue[] args)
        {
            var t = Type.GetType(args[0].String);
            dynamic b = Activator.CreateInstance(t, args[1..].Select(v => v.ToObject()));
            return b;
        }
    }
}
