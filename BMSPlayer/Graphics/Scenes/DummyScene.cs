using Supernova.Core;
using Luminal.Core;
using Luminal.Graphics;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using SFML.Window;
using SFML.System;
using Supernova.Shared;

namespace Supernova.Graphics.Scenes
{
    [SceneDefinition("Dummy")]
    class DummyScene : Scene
    {
        Text t;

        float dt = 0.0f;
        float fps = 0.0f;

        public DummyScene()
        {
            t = new Text
            {
                Font = Globals.Fonts["monospace"],
                DisplayedString = "hi!",
                CharacterSize = 20
            };
        }

        public override void Update(Engine main, float deltaTime)
        {
            dt += deltaTime;
            dt /= 2.0f;
            fps = 1.0f / dt;

            var oa = SNGlobal.Gameplay.Started ?
                string.Format("{0}\n{1}\n{2}\n{3}",
                    SNGlobal.Gameplay.Position, SNGlobal.Gameplay.Beat, SNGlobal.Gameplay.BPM, SNGlobal.Gameplay.bgms.Count)
                : "LOADING CHART...";
            t.DisplayedString = string.Format("{0} fps\n{1}", Math.Floor(fps), oa);
        }

        public override void Draw(Engine main)
        {
            main.Window.Draw(t);
        }

        public override void OnKeyDown(Engine main, KeyEventArgs ea)
        {
            if (ea.Code == Keyboard.Key.Escape)
            {
                main.Quit();
            }
        }
    }
}
