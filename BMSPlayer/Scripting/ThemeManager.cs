using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;
using System.IO;
using Supernova.Scripting.API;
using MoonSharp.Interpreter.Interop;

namespace Supernova.Scripting
{
    public class ThemeManager
    {
        public Script script;
        public string ThemeDirectory;

        public Table ThemeTable;
        public string Name;

        static Dictionary<string, LuaFunction> FunctionMap;

        public ThemeManager(string path)
        {
            UserData.RegistrationPolicy = InteropRegistrationPolicy.Automatic;

            script = new Script();
            var f = File.ReadAllText(path);
            FunctionMap = LuaFunctionFinder.FindLuaFunctions();

            foreach (var (name, value) in FunctionMap)
            {
                Console.WriteLine($"Registering {name}");
                script.Globals[name] = value.GetFunc();
            }

            script.DoString(f);

            CallFunction("OnStart");

            ThemeTable = LuaTable("Theme");
            Name = ThemeTable.Get("Name").String ?? "No name specified!!";
        }
        
        public static ThemeManager LoadTheme(string name)
        {
            var snthemePath = string.Format("Themes/{0}/{0}.sntheme", name);
            var longPath = Path.GetFullPath(snthemePath);
            var snThemeFile = SnthemeReader.ReadSntheme(longPath);
            var themeDir = Path.GetDirectoryName(longPath);
            var entry = Path.Combine(themeDir, snThemeFile.Entrypoint);
            var m = new ThemeManager(entry);
            m.ThemeDirectory = themeDir;
            return m;
        }

        public DynValue LuaCall(string method, object[] args)
        {
            var j = script.Call(script.Globals[method], args);
            return j;
        }

        public DynValue LuaCall(string method)
        {
            var j = script.Call(script.Globals[method]);
            return j;
        }

        public Table LuaTable(string Name)
        {
            Table t = (Table)script.Globals[Name];
            if (t.GetType() != typeof(Table))
            {
                return null;
            }
            return t;
        }

        public DynValue CallFunction(string Name, params object[] args)
        {
            var func = script.Globals.Get(Name);
            if (func.Type != DataType.Function) return null;
            var res = script.Call(func, args);
            return res;
        }

        public void Update(float Delta)
        {
            // Call OnUpdate() here
            CallFunction("OnUpdate", Delta);
        }

        public void Draw()
        {
            CallFunction("OnDraw");
        }
    }
}
