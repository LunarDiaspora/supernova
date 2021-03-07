using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supernova.Configuration;
using Supernova.Gameplay;
using Supernova.Scripting;
using Supernova.BMS;

namespace Supernova.Shared
{
    public static class SNGlobal
    {
        public static GameplayCore Gameplay;
        public static SupernovaConfig Config;
        public static ThemeManager Theme;
        public static BMSChart Chart;
    }
}
