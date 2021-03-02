using Supernova.Core;
using Luminal.Core;
using Luminal.Graphics;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using SFML.Window;
using SFML.System;

namespace Supernova.Graphics.Scenes
{
    [SceneDefinition("Dummy")]
    class DummyScene : Scene
    {
        CircleShape cs;
        Text t;

        float dt = 0.0f;
        float fps = 0.0f;

        float x = 0.0f;

        public DummyScene()
        {
            cs = new CircleShape(100f)
            {
                FillColor = Color.Blue
            };

            t = new Text
            {
                Font = Globals.Fonts["monospace"],
                DisplayedString = "hi!",
                CharacterSize = 12
            };
        }

        public override void Update(Engine main, float deltaTime)
        {
            dt += deltaTime;
            dt /= 2.0f;
            fps = 1.0f / dt;

            t.DisplayedString = string.Format("{0} fps", Math.Floor(fps));

            x += 20 * dt;
            cs.Position = new Vector2f(x, 0f);
        }

        public override void Draw(Engine main)
        {
            main.Window.Draw(cs);
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
