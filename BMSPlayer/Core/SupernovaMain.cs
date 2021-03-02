using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luminal.Core;
using Supernova.BMS;

namespace Supernova.Core
{
    class SupernovaMain
    {
        public Engine engine;

        public SupernovaMain(uint wwidth = 1280, uint wheight = 720, string wtitle = "Supernova")
        {
            engine = new Engine();

            engine.OnLoading += OnEngineLoading;
            engine.OnFinishedLoad += OnEngineLoad;

            engine.StartRenderer(wwidth, wheight, wtitle, typeof(SupernovaMain));
        }

        void OnEngineLoading(Engine main)
        {
            Globals.LoadFont("standard", "Resources/standard.ttf");
            Globals.LoadFont("monospace", "Resources/monospace.ttf");
        }

        void OnEngineLoad(Engine main)
        {
            Console.WriteLine("Supernova has loaded! Let's begin!");

            BMSParser.ParseBMSChart("Songs/freedomdive/dive_n7.bme");

            main.sceneManager.SwitchScene("Dummy");
        }
    }
}
