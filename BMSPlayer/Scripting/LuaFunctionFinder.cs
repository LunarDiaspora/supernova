using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supernova.Scripting.API;
using System.Reflection;
using Luminal.Logging;

namespace Supernova.Scripting
{
    class LuaFunctionFinder
    {
        public static Dictionary<string, LuaFunction> FindLuaFunctions()
        {
            var asm = Assembly.GetExecutingAssembly();
            var n = new Dictionary<string, LuaFunction>();

            foreach (var t in asm.GetTypes())
            {
                object[] attrs = t.GetCustomAttributes(typeof(ExposeToLua), false);
                if (attrs.Length > 1) throw new ArgumentOutOfRangeException($"Expecting 1 ExposeToLua on class {t.Name}, but found {attrs.Length}.");
                if (attrs.Length == 1)
                {
                    var attr = (ExposeToLua)attrs[0];
                    Log.Debug($"Creating Lua function {attr.Name} from class {t.Name}");
                    var fn = (LuaFunction)Activator.CreateInstance(t);
                    n.Add(attr.Name, fn);
                }
            }

            return n;
        }
    }
}
