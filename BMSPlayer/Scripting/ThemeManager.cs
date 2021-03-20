using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;
using System.IO;
using Supernova.Scripting.API;
using MoonSharp.Interpreter.Interop;
using Supernova.BMS;

namespace Supernova.Scripting
{
    public class ThemeManager
    {
        public Script script;
        public string ThemeDirectory;

        public Table ThemeTable;
        public string Name;

        public int NoteXOffset;
        public int NoteYOffset;

        public int NoteWidth;
        public int NoteHeight;

        static Dictionary<string, LuaFunction> FunctionMap;

        public List<Colour> ChartColours = new();

        public string _Text;

        public ThemeManager(string path, bool init = true)
        {
            UserData.RegistrationPolicy = InteropRegistrationPolicy.Automatic;

            script = new Script();
            var f = File.ReadAllText(path);
            _Text = f;
            FunctionMap = LuaFunctionFinder.FindLuaFunctions();

            foreach (var (name, value) in FunctionMap)
            {
                //Console.WriteLine($"Registering {name}");
                script.Globals[name] = value.GetFunc();
            }

            if (init) _Initialise();
        }

        public void _Initialise()
        {
            script.DoString(_Text);

            CallFunction("OnStart");

            ThemeTable = LuaTable("Theme");

            Name = ThemeTable.Get("Name").String ?? "No name specified!!";

            var ctable = ThemeTable.Get("NoteColours").Table;
            var t = ctable.Values.ToList();
            for (int i = 0; i < t.Count; i++)
            {
                var val = t[i];
                var tab = val.Table;
                var r = (int)tab.Get(1).Number;
                var g = (int)tab.Get(2).Number;
                var b = (int)tab.Get(3).Number;
                var a = (int)tab.Get(4).Number;
                var v = new Colour()
                {
                    r = r,
                    g = g,
                    b = b,
                    a = a
                };
                ChartColours.Add(v);
            }

            NoteXOffset = (int)ThemeTable.Get("NoteXOffset").Number;
            NoteYOffset = (int)ThemeTable.Get("NoteYOffset").Number;

            NoteWidth = (int)ThemeTable.Get("NoteWidth").Number;
            NoteHeight = (int)ThemeTable.Get("NoteHeight").Number;
        }

        public static ThemeManager LoadTheme(string name, bool init = true)
        {
            var snthemePath = string.Format("Themes/{0}/{0}.sntheme", name);
            var longPath = Path.GetFullPath(snthemePath);
            var snThemeFile = SnthemeReader.ReadSntheme(longPath);
            var themeDir = Path.GetDirectoryName(longPath);
            var entry = Path.Combine(themeDir, snThemeFile.Entrypoint);
            var m = new ThemeManager(entry, init);
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

        public void OnChartLoaded()
        {
            CallFunction("OnChartLoad");
        }

        public void DrawAfterNotes()
        {
            CallFunction("DrawAfterNotes");
        }

        public void OnJudgement(JudgementData j)
        {
            CallFunction("OnJudgement", j);
        }
    }
}
