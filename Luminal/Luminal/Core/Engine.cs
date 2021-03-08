using Luminal.Graphics;
using Luminal.Audio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Luminal.Configuration;
using SDL2;
using SFML.System;

namespace Luminal.Core
{
    public class Engine
    {
        public static IntPtr Renderer; // SDL_Renderer*
        public static IntPtr Window; // SDL_Window*
        public SceneManager sceneManager;

        public static bool WindowOpen;

        public Clock sfClock;

        public delegate void FinishedLoadCallback(Engine main);
        public event FinishedLoadCallback OnFinishedLoad;

        public delegate void LoadingCallback(Engine main);
        public event LoadingCallback OnLoading;

        public delegate void UpdateCallback(Engine main, float Delta);
        public event UpdateCallback OnUpdate;

        public delegate void DrawCallback(Engine main);
        public event DrawCallback OnDraw;

        public void StartRenderer(int WindowWidth, int WindowHeight, string WindowTitle, Type executingType)
        {
            Console.WriteLine($"--- Luminal Engine ---\nStarting at {WindowWidth} x {WindowHeight} (\"{WindowTitle}\")\nExecuting application: {executingType.Name}\n");

            var config = LuminalConfigLoader.LoadConfig("Luminal.json");

            AudioEngineManager.LoadEngine(config.AudioPlugin);

            sceneManager = new SceneManager(executingType);
            //sceneManager.SwitchScene("Dummy");

            Window = SDL.SDL_CreateWindow(WindowTitle, 200, 200, WindowWidth, WindowHeight, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
            Renderer = SDL.SDL_CreateRenderer(Window, 0, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

            Context.SetColour(255, 255, 255, 255);

            Console.WriteLine("Loading SDL2_ttf");
            SDL_ttf.TTF_Init();

            Console.WriteLine("Loading SDL2_image");
            SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_JPG | SDL_image.IMG_InitFlags.IMG_INIT_PNG |
                               SDL_image.IMG_InitFlags.IMG_INIT_TIF | SDL_image.IMG_InitFlags.IMG_INIT_WEBP);

            OnLoading(this);

            //var sdlResult = SDL.SDL_CreateWindowAndRenderer(WindowWidth, WindowHeight, 0, out Renderer, out Window);
            //Console.WriteLine($"{sdlResult}");
            //SDL.SDL_SetWindowTitle(Window, WindowTitle);

            //Window.SetFramerateLimit(500);

            OnFinishedLoad(this);
            WindowOpen = true;

            sfClock = new Clock();

            while (WindowOpen)
            {
                SDL.SDL_SetRenderDrawColor(Renderer, 0, 0, 0, 255);
                SDL.SDL_RenderClear(Renderer);
                SDL.SDL_SetRenderDrawColor(Renderer, 255, 255, 255, 255);

                SDL.SDL_Event evt;
                while (SDL.SDL_PollEvent(out evt) == 1)
                {
                    switch (evt.type)
                    {
                        case SDL.SDL_EventType.SDL_QUIT:
                            WinClose();
                            break;
                    }
                }

                var t = sfClock.Restart();

                AudioEngineManager.Engine.Update(t.AsSeconds());

                if (sceneManager.ActiveScene != null)
                    sceneManager.ActiveScene.Update(this, t.AsSeconds());

                OnUpdate(this, t.AsSeconds());

                if (sceneManager.ActiveScene != null) 
                    sceneManager.ActiveScene.Draw(this);

                OnDraw(this);

                SDL.SDL_RenderPresent(Renderer);

                //SDL.SDL_Delay(1);
            }
        }

        public void Quit()
        {
            AudioEngineManager.Engine.Dispose(); // Clean up after ourselves

            WindowOpen = false;
            SDL.SDL_DestroyWindow(Window);
            SDL.SDL_DestroyRenderer(Renderer);

            Context.FreeAllImages();
        }

        private void WinKeyDown()
        {
            sceneManager.ActiveScene.OnKeyDown(this);
        }

        private void WinKeyUp()
        {
            sceneManager.ActiveScene.OnKeyUp(this);
        }

        private void WinClose()
        {
            Quit();
        }
    }
}
