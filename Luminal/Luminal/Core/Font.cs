﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace Luminal.Core
{
    public class Font
    {
        // Wrapper class for SDL2 fonts
        public IntPtr FontPointer;

        public Font(string Path, int Size = 12)
        {
            FontPointer = SDL_ttf.TTF_OpenFont(Path, Size);
            if (FontPointer == IntPtr.Zero)
            {
                Console.WriteLine("Something went wrong with TTF_OpenFont.");
                string Err = SDL.SDL_GetError();
                Console.WriteLine(Err);
            }
        }

        public void RawDraw(string Text, int x = 0, int y = 0)
        {
            SDL.SDL_Rect rect = new();
            rect.x = x;
            rect.y = y;

            SDL_ttf.TTF_SizeUTF8(FontPointer, Text, out rect.w, out rect.h);

            IntPtr sur = SDL_ttf.TTF_RenderUTF8_Shaded(FontPointer, Text, Context.Colour, Context.Black);
            // SDL_Surface*
            //SDL.SDL_BlitSurface(sur, IntPtr.Zero, Engine.Renderer, ref rect);
            IntPtr tex = SDL.SDL_CreateTextureFromSurface(Engine.Renderer, sur);
            var e = SDL.SDL_RenderCopy(Engine.Renderer, tex, IntPtr.Zero, ref rect);
            SDL.SDL_FreeSurface(sur);
            SDL.SDL_DestroyTexture(tex);
        }

        public void Draw(string Text, int x = 0, int y = 0)
        {
            int fonty = 0;
            SDL_ttf.TTF_SizeUTF8(FontPointer, Text, out _, out fonty);
            var j = Text.Split("\n");
            for (int k=0; k<j.Length; k++)
            {
                RawDraw(j[k], x, y + (fonty * k));
            }
        }
    }
}