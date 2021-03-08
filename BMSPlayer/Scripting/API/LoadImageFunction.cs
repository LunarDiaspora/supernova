using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luminal.Core;

namespace Supernova.Scripting.API
{
    [ExposeToLua("SN_LoadImage")]
    class LoadImageFunction : LuaFunction
    {
        public override DynValue Execute(params DynValue[] args)
        {
            var path = args[0].String;
            var ok = Image.LoadFrom(path, out Image i);
            if (!ok)
            {
                throw new ScriptRuntimeException($"SN_LoadImage: Failed to load image from path ${path}.");
            }

            var li = new LuaImage(i);
            return UserData.Create(li);
        }
    }
}
