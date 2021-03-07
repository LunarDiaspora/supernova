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
        public override DynValue Execute(params DynValue[] args)
        {
            var _tbl = new Table(SNGlobal.Theme.script);
            var _chart = SNGlobal.Chart.GetAllNotes();

            // Temporary
            for (int i=0; i<_chart.Count; i++)
            {
                var g = _chart[i];
                _tbl.Set(i, UserData.Create(g));
            }

            return DynValue.NewTable(_tbl);
        }
    }
}
