using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supernova.Shared;

namespace Supernova.Scripting.API
{
    [ExposeToLua("SN_GetGameplay")]
    class GetGameplay : LuaFunction
    {
        public override DynValue Execute(params DynValue[] args)
        {
            return UserData.Create(SNGlobal.Gameplay);
        }
    }
}
