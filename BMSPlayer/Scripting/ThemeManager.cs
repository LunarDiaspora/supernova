using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Supernova.Scripting.API;
using Supernova.BMS;
using Neo.IronLua;
using Supernova.Gameplay;

namespace Supernova.Scripting
{
    public class ThemeManager
    {
        public Lua LuaScript;
        public dynamic LuaEnvironment;
        public string ThemeDirectory;

        public dynamic ThemeTable;
        public string Name;

        public int NoteXOffset;
        public int NoteYOffset;

        public int NoteWidth;
        public int NoteHeight;

        static Dictionary<string, dynamic> FunctionMap;

        public List<Colour> ChartColours = new();

        public string _Text;
        public string Style;

        public LuaChunk chunk;

        public ThemeManager(string path, bool init = true)
        {
            LuaScript = new Lua();
            LuaEnvironment = LuaScript.CreateEnvironment<LuaGlobal>();

            var f = File.ReadAllText(path);
            _Text = f;
            FunctionMap = LuaFunctionFinder.FindLuaFunctions();

            foreach (var (name, value) in FunctionMap)
            {
                //Console.WriteLine($"Registering {name}");
                LuaEnvironment[name] = value.GetFunc;
            }

            chunk = LuaScript.CompileChunk(_Text, Path.GetFileName(path), new LuaCompileOptions()
            {
                DebugEngine = LuaExceptionDebugger.Default
            });

            if (init) _Initialise();
        }

        public void _Initialise()
        {
            chunk.Run(LuaEnvironment);

            CallFunction("OnStart");

            ThemeTable = LuaEnvironment.Theme;

            Name = ThemeTable.Name ?? "No name specified.";
            Style = ThemeTable.Style;

            if (Style == "Play")
            {
                var ct = ThemeTable.NoteColours;
                var t = ct;
                for (int i = 0; i < t.Count; i++)
                {
                    var val = t[i];
                    var tab = val.Table;
                    var r = (int)tab[1];
                    var g = (int)tab[2];
                    var b = (int)tab[3];
                    var a = (int)tab[4];
                    var v = new Colour()
                    {
                        r = r,
                        g = g,
                        b = b,
                        a = a
                    };
                    ChartColours.Add(v);
                }

                NoteXOffset = (int)ThemeTable.NoteXOffset;
                NoteYOffset = (int)ThemeTable.NoteYOffset;

                NoteWidth = (int)ThemeTable.NoteWidth;
                NoteHeight = (int)ThemeTable.NoteHeight;
            }
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

        public dynamic CallFunction(string method)
        {
            var j = LuaEnvironment[method]();
            return j;
        }

        public dynamic CallFunction(string method, object arg)
        {
            var j = LuaEnvironment[method](arg);
            return j;
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

        public void OnJudgement(Judgement j)
        {
            CallFunction("OnJudgement", j);
        }
    }
}
