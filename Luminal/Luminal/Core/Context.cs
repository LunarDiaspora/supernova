﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace Luminal.Core
{
    public class Context
    {
        private static SDL.SDL_Color IntColour;
        public static SDL.SDL_Color Colour { get { return IntColour; } }

        public static SDL.SDL_Color Black = MakeColourFromRGBA(0, 0, 0, 255);

        public static SDL.SDL_Color MakeColourFromRGBA(byte r, byte g, byte b, byte a = 255)
        {
            return new SDL.SDL_Color
            {
                r = r,
                g = g,
                b = b,
                a = a
            };
        }

        public static void SetColour(byte r, byte g, byte b, byte a = 255)
        {
            IntColour = MakeColourFromRGBA(r, g, b, a);
        }
    }
}
