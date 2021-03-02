using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Luminal.Core
{
    public class Globals
    {
        public static Dictionary<string, Font> Fonts = new Dictionary<string, Font>();

        public static void LoadFont(string fontName, string fontPath)
        {
            Font f = new Font(fontPath);
            Fonts.Add(fontName, f);
        }
    }
}
