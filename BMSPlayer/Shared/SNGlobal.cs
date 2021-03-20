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

        public static Dictionary<string, ThemeManager> Themes = new();

        public static ThemeManager LoadTheme(string Key, string Name)
        {
            var t = ThemeManager.LoadTheme(Name, false);
            Themes[Key] = t;
            return t;
        }

        public static void SwitchTheme(string Key)
        {
            Theme = Themes[Key];
            Theme._Initialise();
        } 

        public static BMSChart Chart;
    }
}
