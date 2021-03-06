using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;

namespace Supernova.Scripting.API
{
    [ExposeToLua("LuaTest")]
    class TestLuaFunction : LuaFunction
    {
        public override DynValue Execute(params DynValue[] args)
        {
            Console.WriteLine("LuaTest()");
            return null;
        }
    }
}
