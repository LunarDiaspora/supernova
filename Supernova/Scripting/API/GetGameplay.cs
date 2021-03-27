using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supernova.Gameplay;
using Supernova.Shared;

namespace Supernova.Scripting.API
{
    [ExposeToLua("SN_GetGameplay")]
    class GetGameplay
    {
        public GameplayCore Execute()
        {
            return SNGlobal.Gameplay;
        }

        public Func<GameplayCore> GetFunc => Execute;
    }
}
