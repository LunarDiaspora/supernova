using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luminal.Core;
using Supernova.BMS;
using Supernova.Gameplay;
using Supernova.Shared;
using Supernova.Configuration;
using Supernova.Scripting;
using DiscordRPC;
using DiscordRPC.Logging;
using Luminal.Logging;
using Supernova.Disk;
using CommandLine;
using CommandLine.Text;

namespace Supernova.Core
{
    class CommandOptions
    {
        [Option('l', "loglevel", HelpText = "The verbosity level of the logging.", Default = 0)]
        public int LogLevel { get; set; }
        [Option('c', "chart", HelpText = "The chart hash of the chart to play when starting.", Default = null)]
        public string Chart { get; set; }
        [Option("autoplay", Default = false, HelpText = "Plays charts automatically.")]
        public bool AutoPlay { get; set; }
        [Option("noRPC", Default = false, HelpText = "Disables the Discord integration.")]
        public bool NoRPC { get; set; }

        [Usage(ApplicationAlias = "Supernova")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                return new List<Example>()
                {
                };
            }
        }
    }

    class SupernovaMain
    {
        public static Engine engine;
        public static DiscordRpcClient RPC;

        public static readonly string SUPERNOVA_CLIENT_ID = "820302107026391060";

        public static string BaseTitle;

        public static CommandOptions args;

        public static SupernovaState State;

        public static Dictionary<string, Folder> SongFolders;

        public SupernovaMain(int wwidth = 1280, int wheight = 720, string wtitle = "Supernova", CommandOptions a = null)
        {
            State = SupernovaState.LOADING;
            
            SNGlobal.Config = SupernovaConfigLoader.LoadConfig("Supernova.json");

            args = a;
            if (!(bool)args?.NoRPC)
            {
                RPC = new DiscordRpcClient(SUPERNOVA_CLIENT_ID);
                RPC.Logger = new DiscordRPC.Logging.ConsoleLogger()
                {
                    Level = DiscordRPC.Logging.LogLevel.Warning
                };

                RPC.OnReady += (sender, e) =>
                {
                    Log.Info("Discord RPC: Ready on user {0}", e.User.Username);
                };
            }

            BaseTitle = wtitle;
            engine = new Engine((int)args?.LogLevel);

            engine.OnLoading += OnEngineLoading;
            engine.OnFinishedLoad += OnEngineLoad;
            engine.OnUpdate += OnEngineUpdate;
            engine.OnDraw += OnEngineDraw;

            // Logger test
            //Log.Debug("Debug level");
            //Log.Info("Info level");
            //Log.Warn("Warning level");
            //Log.Error("Error level");
            //Log.Fatal("Critical level!");
            //Log.Wtf("'Something very wrong has happened' level!");

            engine.StartRenderer(wwidth, wheight, wtitle, typeof(SupernovaMain));
        }

        void OnEngineLoading(Engine main)
        {
            if (RPC != null) RPC.Initialize();

            Globals.LoadFont("standard", "Resources/standard.ttf", 16);
            Globals.LoadFont("monospace", "Resources/monospace.ttf", 24);

            var a = Context.LoadImage("test", "Themes/Default/key.png");
            //Log.Debug("{0}", a);
        }

        void OnEngineLoad(Engine main)
        {
            Log.Info("Supernova has loaded! Let's begin!");

            foreach (var (key, theme) in SNGlobal.Config.Theme)
            {
                Log.Debug($"Theme '{key}' = {theme}");

                SNGlobal.LoadTheme(key, theme);
                Log.Debug($"Theme '{key}' loaded successfully, but not initialised yet.");
            }
            
            //BMSParser.ParseBMSChart("Songs/freedomdive/dive_n7.bme");
            SNGlobal.Gameplay = new GameplayCore();
            //SNGlobal.Gameplay.LoadGameplay("Songs/gengaozo/gengaozo_foon_f.bme");

            SongFolders = ChartFinder.FindCharts();

            main.sceneManager.SwitchScene("Main");

            State = SupernovaState.SONG_SELECT;

            SNGlobal.SwitchTheme("SongSelect");
            Log.Debug($"Lua: Song-select theme name is '{SNGlobal.Theme.Name}'.");
        }

        void OnEngineUpdate(Engine main, float Delta)
        {
            if (SNGlobal.Gameplay != null)
            {
                SNGlobal.Gameplay.UpdateEngine(Delta);
            }

            if (SNGlobal.Theme != null)
            {
                SNGlobal.Theme.Update(Delta);
            }
        }

        void OnEngineDraw(Engine main)
        {
            if (SNGlobal.Theme != null)
            {
                SNGlobal.Theme.Draw();
            }
        }
    }
}
