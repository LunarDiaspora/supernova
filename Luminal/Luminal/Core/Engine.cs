using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Luminal.Graphics;
using Luminal.Audio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Luminal.Configuration;

namespace Luminal.Core
{
    public class Engine
    {
        public VideoMode Mode;
        public RenderWindow Window;
        public SceneManager sceneManager;

        public Clock sfClock;

        public delegate void FinishedLoadCallback(Engine main);
        public event FinishedLoadCallback OnFinishedLoad;

        public delegate void LoadingCallback(Engine main);
        public event LoadingCallback OnLoading;

        public delegate void UpdateCallback(Engine main, float Delta);
        public event UpdateCallback OnUpdate;

        public void StartRenderer(uint WindowWidth, uint WindowHeight, string WindowTitle, Type executingType)
        {
            Console.WriteLine($"--- Luminal Engine ---\nStarting at {WindowWidth} x {WindowHeight} (\"{WindowTitle}\")\nExecuting application: {executingType.Name}\n");

            var config = LuminalConfigLoader.LoadConfig("Luminal.json");

            AudioEngineManager.LoadEngine(config.AudioPlugin);

            OnLoading(this);

            sceneManager = new SceneManager(executingType);
            //sceneManager.SwitchScene("Dummy");

            Mode = new VideoMode(WindowWidth, WindowHeight);
            Window = new RenderWindow(Mode, WindowTitle);

            //Window.SetFramerateLimit(500);

            Window.KeyPressed += WinKeyDown;
            Window.KeyReleased += WinKeyUp;
            
            Window.Closed += WinClose;

            sfClock = new Clock();

            OnFinishedLoad(this);

            while (Window.IsOpen)
            {
                Window.DispatchEvents();

                Time t = sfClock.Restart();

                AudioEngineManager.Engine.Update(t.AsSeconds());

                if (sceneManager.ActiveScene != null)
                    sceneManager.ActiveScene.Update(this, t.AsSeconds());

                OnUpdate(this, t.AsSeconds());

                Window.Clear(Color.Black);

                if (sceneManager.ActiveScene != null) 
                    sceneManager.ActiveScene.Draw(this);

                Window.Display();
            }
        }

        public void Quit()
        {
            AudioEngineManager.Engine.Dispose(); // Clean up after ourselves

            Window.Close();
        }

        private void WinKeyDown(object sender, KeyEventArgs ea)
        {
            sceneManager.ActiveScene.OnKeyDown(this, ea);
        }

        private void WinKeyUp(object sender, KeyEventArgs ea)
        {
            sceneManager.ActiveScene.OnKeyUp(this, ea);
        }

        private void WinClose(object sender, EventArgs ea)
        {
            Quit();
        }
    }
}
