using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;

namespace Supernova.Scripting.API
{
    class LuaFunction
    {
        public virtual DynValue Execute(params DynValue[] args)
        {
            // Do things here
            return null;
        }

        public Func<DynValue[], DynValue> GetFunc()
        {
            return Execute;
        }
    }
}
