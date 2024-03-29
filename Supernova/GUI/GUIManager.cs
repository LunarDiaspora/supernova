﻿using Luminal.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using Supernova.Shared;
using Supernova.Core;
using System.Numerics;
using Supernova.Gameplay;
using Supernova.Configuration;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.IO;

namespace Supernova.GUI
{
    unsafe class GUIManager
    {
        public static bool LoadBMSWindowOpen = false;
        public static bool IsSystemMenuOpen = false;
        public static bool DiagnosticsOpen = false;
        public static bool OptionsWindowOpen = false;
        public static float dt = 0.0f;

        static string BMSPathBuffer = "";

        static Vector4 COLOUR_ERROR = new(219f, 53f, 53f, 255f);

        public static unsafe void GUI(Engine ___)
        {
            if (IsSystemMenuOpen)
            {
                ImGui.BeginMainMenuBar();
                ImGui.Text("System Menu");
                ImGui.Spacing();
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("Load BMS from path..."))
                    {
                        LoadBMSWindowOpen = true;
                    }
                    ImGui.EndMenu();
                }
                if (ImGui.BeginMenu("Game"))
                {
                    if (ImGui.MenuItem("Game options")) OptionsWindowOpen = true;
                    ImGui.EndMenu();
                }
                if (ImGui.BeginMenu("Debugging"))
                {
                    if (ImGui.MenuItem("Diagnostics"))
                    {
                        DiagnosticsOpen = true;
                    }
                    ImGui.EndMenu();
                }
                ImGui.EndMainMenuBar();
            }

            if (LoadBMSWindowOpen)
            {
                ImGui.Begin("Load BMS from File Path", ref LoadBMSWindowOpen, ImGuiWindowFlags.NoCollapse);

                ImGui.InputText("Path", ref BMSPathBuffer, 65536);
                if (ImGui.Button("Load!"))
                {
                    if (File.Exists(BMSPathBuffer))
                    {
                        GameplayCore.ResetAndLoad(BMSPathBuffer);
                    } else
                    {
                        ImGui.OpenPopup("No file at path");
                    }
                }

                bool _ = true;

                var center = ImGui.GetMainViewport().GetCenter();
                ImGui.SetNextWindowPos(center, ImGuiCond.Appearing, new Vector2(0.5f, 0.5f));

                if (ImGui.BeginPopupModal("No file at path", ref _, ImGuiWindowFlags.AlwaysAutoResize))
                {
                    ImGui.Text("There is no file at this location.\nCheck that your path is correct.");
                    ImGui.Text("\n");
                    if (ImGui.Button("OK", new Vector2(240, 0)))
                    {
                        ImGui.CloseCurrentPopup();
                    }

                    ImGui.EndPopup();
                }

                ImGui.End();
            }

            if (DiagnosticsOpen)
            {
                ImGui.Begin("Diagnostics", ref DiagnosticsOpen, ImGuiWindowFlags.NoCollapse);

                ImGui.Text($"Framerate (FPS): {1 / dt}");

                if (ImGui.TreeNode("Gameplay statistics"))
                {
                    if (SNGlobal.Gameplay == null)
                    {
                        ImGui.TextColored(COLOUR_ERROR, "Gameplay object does not exist. This should NEVER happen!");
                    } else
                    {
                        if (SNGlobal.Gameplay.Started)
                        {
                            var ch = SNGlobal.Gameplay.Chart;
                            var title = $"{ch.title} {ch.subtitle ?? ""}";
                            ImGui.Text($"{ch.artist} - {title}");
                            ImGui.Text($"Difficulty: {ch.difficulty} lv. {ch.playLevel}");
                            ImGui.Text($"Chart hash: {ch.SHA256_Hash}");
                            ImGui.Text($"{ch.Samples.Count} samples loaded.");
                        } else
                        {
                            ImGui.Text("No chart loaded, or chart is currently loading.");
                        }
                    }
                }

                ImGui.End();
            }

            if (OptionsWindowOpen)
            {
                ImGui.Begin("Game Options", ref OptionsWindowOpen, ImGuiWindowFlags.NoCollapse);

                if (ImGui.TreeNode("Regular options"))
                {
                    ImGui.SliderFloat("Hi-Speed", ref GameplayOptions.Instance.UserHighSpeed, 0, 15, "%f");

                    ImGui.DragFloat("Offset", ref GameplayOptions.Instance.Offset, 0.01f);

                    ImGui.Checkbox("Autoplay", ref GameplayCore.Autoplay);

                    ImGui.TreePop();
                }

                if (ImGui.TreeNode("Stupid options"))
                {
                    ImGui.Checkbox("Wave", ref GameplayOptions.Instance.Wave);
                    ImGui.SliderFloat("Wave Period", ref GameplayOptions.Instance.WavePeriod, 1f, 5f);
                    ImGui.SliderFloat("Wave Scale", ref GameplayOptions.Instance.WaveScale, 0f, 5f);

                    ImGui.TreePop();
                }

                if (ImGui.Button("Save configuration"))
                {
                    GameplayConfig.Save();
                }

                ImGui.End();
            }
        }

        public static void HandleKeypress(SDL2.SDL.SDL_Scancode sc)
        {
            if (sc == SDL2.SDL.SDL_Scancode.SDL_SCANCODE_F1)
            {
                // Toggle menu!
                IsSystemMenuOpen = !IsSystemMenuOpen;
            }
        }

        public static void Update(float t)
        {
            dt += t;
            dt /= 2;
        }
    }
}
