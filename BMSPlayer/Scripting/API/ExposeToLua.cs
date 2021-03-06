using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supernova.Scripting.API
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    class ExposeToLua : Attribute
    {
        public string Name;

        public ExposeToLua(string n)
        {
            Name = n;
        }
    }
}
