using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supernova.Shared;

namespace Supernova.Scripting.API
{
    [ExposeToLua("SN_GetNotes")]
    class GetNotes : LuaFunction
    {
        static Table NoteCache = new(SNGlobal.Theme.script);

        public override DynValue Execute(params DynValue[] args)
        {
            NoteCache.Clear();
            var _chart = SNGlobal.Chart.GetAllNotes();

            // THIS IS SLOW!
            // AVOID calling this every frame!
            for (int i = 0; i < _chart.Count; i++)
            {
                var g = _chart[i];
                NoteCache.Set(i, UserData.Create(g));
            }

            return DynValue.NewTable(NoteCache);
        }
    }
}
