using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;
using Supernova.Gameplay;
using MoonSharp.Interpreter.Interop;

namespace Supernova.Scripting.API
{
    [MoonSharpUserData]
    public class JudgementData
    {
        [MoonSharpHidden]
        public Judgement original;

        [MoonSharpVisible(true)]
        public int judgement;
    }
}
