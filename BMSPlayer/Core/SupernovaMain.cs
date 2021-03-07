﻿using System;
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

namespace Supernova.Core
{
    class SupernovaMain
    {
        public Engine engine;

        public SupernovaMain(int wwidth = 1280, int wheight = 720, string wtitle = "Supernova")
        {
            SNGlobal.Config = SupernovaConfigLoader.LoadConfig("Supernova.json");

            engine = new Engine();

            engine.OnLoading += OnEngineLoading;
            engine.OnFinishedLoad += OnEngineLoad;
            engine.OnUpdate += OnEngineUpdate;
            engine.OnDraw += OnEngineDraw;

            engine.StartRenderer(wwidth, wheight, wtitle, typeof(SupernovaMain));
        }

        void OnEngineLoading(Engine main)
        {
            Globals.LoadFont("standard", "Resources/standard.ttf", 16);
            Globals.LoadFont("monospace", "Resources/monospace.ttf", 24);
        }

        void OnEngineLoad(Engine main)
        {
            Console.WriteLine("Supernova has loaded! Let's begin!");
            Console.WriteLine($"Loading theme {SNGlobal.Config.Theme}...");

            SNGlobal.Theme = ThemeManager.LoadTheme(SNGlobal.Config.Theme);
            Console.WriteLine($"Lua loaded: theme name = {SNGlobal.Theme.Name}");

            //BMSParser.ParseBMSChart("Songs/freedomdive/dive_n7.bme");
            SNGlobal.Gameplay = new GameplayCore();
            SNGlobal.Gameplay.LoadGameplay("Songs/freedomdive/dive_n7.bme");

            main.sceneManager.SwitchScene("Main");
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
