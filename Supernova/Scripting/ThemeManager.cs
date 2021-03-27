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
using Luminal.Logging;
using Supernova.Shared;

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

        public string FilePath;

        public LuaChunk chunk;

        public bool Error = false;

        public ThemeManager(string path, bool init = true)
        {
            FilePath = path;

            LuaScript = new Lua();

            var f = File.ReadAllText(path);
            _Text = f;
            FunctionMap = LuaFunctionFinder.FindLuaFunctions();

            try
            {
                chunk = LuaScript.CompileChunk(_Text, Path.GetFileName(path), new LuaCompileOptions()
                {
                    DebugEngine = LuaExceptionDebugger.Default
                });
            } catch(LuaParseException e)
            {
                var estr = $"Lua compile error ({Path.GetFileName(path)}): (at line {e.Line}:{e.Column}) {e.Message}\n";
                Log.Error("");
                Log.Error(estr);
                Error = true;
                return;
            }

            if (init) _Initialise();
        }

        public void _Initialise()
        {
            if (Error) return;
            LuaEnvironment = LuaScript.CreateEnvironment<LuaGlobal>();

            LuaEnvironment.Log = LuaType.GetType(typeof(Log));
            LuaEnvironment.SDL = LuaType.GetType(typeof(SDL2.SDL));
            LuaEnvironment.Global = LuaType.GetType(typeof(SNGlobal));

            foreach (var (name, value) in FunctionMap)
            {
                //Console.WriteLine($"Registering {name}");
                LuaEnvironment[name] = value.GetFunc;
            }

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
            if (Error) return null;

            try
            {
                var j = LuaEnvironment[method]();
                return j;
            } catch (LuaRuntimeException e)
            {
                var estr = $"Lua runtime error ({Path.GetFileName(FilePath)}): {e.Message}";
                Log.Error("");
                Log.Error(estr);
                Log.Error(e.StackTrace);
                Error = true;
                return null;
            }
        }

        public dynamic CallFunction(string method, dynamic arg)
        {
            if (Error) return null;

            if (LuaEnvironment[method] != null)
            {
                var j = LuaEnvironment[method](arg);
                return j;
            } else
            {
                return null;
            }
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

        public void KeyDown(SDL2.SDL.SDL_Scancode k)
        {
            CallFunction("OnKeyDown", _ScancodeToString(k));
        }

        public void KeyUp(SDL2.SDL.SDL_Scancode k)
        {
            CallFunction("OnKeyUp", _ScancodeToString(k));
        }

        public string _ScancodeToString(SDL2.SDL.SDL_Scancode k)
        {
            var s = "";
            var i = GameplayCore.Keymap.IndexOf(k);
            if (i != -1)
            {
                switch (i)
                {
                    case 0:
                        s = "Scratch";
                        break;
                    default:
                        s = $"Key{i}";
                        break;
                }
                return s;
            }
            return k.ToString();
        }
    }
}
