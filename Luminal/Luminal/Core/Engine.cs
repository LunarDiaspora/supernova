﻿using Luminal.Graphics;
using Luminal.Audio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Luminal.Configuration;
using SDL2;
using SFML.System;
using Luminal.Logging;

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

        public static int Width;
        public static int Height;

        public Engine(int logLevel = 0)
        {
            var logger = new ConsoleLogger();
            Log.SetLogger(logger);
            Log.SetLogLevel(logLevel);
        }

        public void StartRenderer(int WindowWidth, int WindowHeight, string WindowTitle, Type executingType)
        {
            Log.Info($"--- Luminal Engine ---\nStarting at {WindowWidth} x {WindowHeight} (\"{WindowTitle}\")\nExecuting application: {executingType.Name}\n");

            Width = WindowWidth;
            Height = WindowHeight;

            var config = LuminalConfigLoader.LoadConfig("Luminal.json");

            AudioEngineManager.LoadEngine(config.AudioPlugin);

            sceneManager = new SceneManager(executingType);
            //sceneManager.SwitchScene("Dummy");

            Window = SDL.SDL_CreateWindow(WindowTitle, 200, 200, WindowWidth, WindowHeight, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN);
            Renderer = SDL.SDL_CreateRenderer(Window, 0, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

            Context.SetColour(255, 255, 255, 255);

            Log.Info("Loading SDL2_ttf");
            SDL_ttf.TTF_Init();

            Log.Info("Loading SDL2_image");
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
                        case SDL.SDL_EventType.SDL_KEYDOWN:
                            var e = evt.key.keysym.scancode;
                            WinKeyDown(e);
                            break;
                        case SDL.SDL_EventType.SDL_KEYUP:
                            var k = evt.key.keysym.scancode;
                            WinKeyUp(k);
                            break;
                    }
                }

                var t = sfClock.Restart();

                AudioEngineManager.Engine.Update(t.AsSeconds());

                if (sceneManager.ActiveScene != null)
                    sceneManager.ActiveScene.Update(this, t.AsSeconds());

                OnUpdate(this, t.AsSeconds());

                OnDraw(this);

                if (sceneManager.ActiveScene != null) 
                    sceneManager.ActiveScene.Draw(this);

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

        private void WinKeyDown(SDL.SDL_Scancode scancode)
        {
            sceneManager.ActiveScene.OnKeyDown(this, scancode);
        }

        private void WinKeyUp(SDL.SDL_Scancode scancode)
        {
            sceneManager.ActiveScene.OnKeyUp(this, scancode);
        }

        private void WinClose()
        {
            Quit();
        }
    }
}
