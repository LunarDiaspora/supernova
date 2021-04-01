using Luminal.Core;
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
        public static float dt = 0.0f;

        static string BMSPathBuffer = "";

        static Vector4 COLOUR_ERROR = new(219f, 53f, 53f, 255f);

        static IntPtr PTR_HISPEED = new IntPtr(Unsafe.AsPointer(ref GameplayOptions.HighSpeed));

        static bool FloatSlider(string name, IntPtr variable, float min, float max, string fmt = "%f")
        {
            var zero = min;
            var ten = max;

            var zero_p = new IntPtr(Unsafe.AsPointer(ref zero));
            var ten_p = new IntPtr(Unsafe.AsPointer(ref ten));

            return ImGui.SliderScalar(name, ImGuiDataType.Float, variable, zero_p, ten_p, fmt);
        }

        public static unsafe void GUI(Engine ___)
        {
            if (IsSystemMenuOpen)
            {
                ImGui.BeginMainMenuBar();
                ImGui.Text("System Menu");
                ImGui.Spacing();
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("Load BMS From Path..."))
                    {
                        LoadBMSWindowOpen = true;
                    }
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

                            FloatSlider("Hi-Speed", PTR_HISPEED, 0, 15, "%f");
                        } else
                        {
                            ImGui.Text("No chart loaded, or chart is currently loading.");
                        }
                    }
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
