using Luminal.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using Supernova.Shared;
using Supernova.Core;

namespace Supernova.GUI
{
    class GUIManager
    {
        public static bool LoadBMSWindowOpen = false;
        public static bool IsSystemMenuOpen = false;
        public static bool DiagnosticsOpen = false;
        public static float dt = 0.0f;

        public static void GUI(Engine _)
        {
            //if (IsSystemMenuOpen)
            //{
            //    ImGui.BeginMainMenuBar();
            //    ImGui.Text("System Menu");
            //    ImGui.Spacing();
            //    if (ImGui.BeginMenu("File"))
            //    {
            //        if (ImGui.MenuItem("Load BMS From Path..."))
            //        {
            //            LoadBMSWindowOpen = true;
            //        }
            //        ImGui.EndMenu();
            //    }
            //    if (ImGui.BeginMenu("Debugging"))
            //    {
            //        if (ImGui.MenuItem("Diagnostics"))
            //        {
            //            DiagnosticsOpen = true;
            //        }
            //        ImGui.EndMenu();
            //    }
            //    ImGui.EndMainMenuBar();
            //}

            //if (LoadBMSWindowOpen)
            //{
            //    ImGui.Begin("Load BMS from File Path", ImGuiWindowFlags.NoCollapse);

            //    string buf = "";
            //    ImGui.InputText("Path", ref buf, 65536);
            //    if (ImGui.Button("Load!"))
            //    {
            //        SNGlobal.SwitchTheme("Play");
            //        SupernovaMain.State = SupernovaState.PLAY;

            //        SNGlobal.Gameplay.LoadGameplay(buf);
            //    }

            //    if (ImGui.Button("Close")) LoadBMSWindowOpen = false;

            //    ImGui.End();
            //}

            //if (DiagnosticsOpen)
            //{
            //    ImGui.Begin("Diagnostics", ImGuiWindowFlags.NoCollapse);

            //    ImGui.Text($"Framerate (FPS): {1 / dt}");
            //    if (ImGui.Button("Close")) DiagnosticsOpen = false;

            //    ImGui.End();
            //}
        }

        public static void HandleKeypress(SDL2.SDL.SDL_Scancode sc)
        {
            //if (sc == SDL2.SDL.SDL_Scancode.SDL_SCANCODE_F1)
            //{
            //    // Toggle menu!
            //    IsSystemMenuOpen = !IsSystemMenuOpen;
            //}
        }

        public static void Update(float t)
        {
            dt += t;
            dt /= 2;
        }
    }
}
