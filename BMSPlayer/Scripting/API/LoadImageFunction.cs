using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luminal.Core;

namespace Supernova.Scripting.API
{
    [ExposeToLua("SN_LoadImage")]
    class LoadImageFunction
    {
        public Image Execute(string path)
        {
            var ok = Image.LoadFrom(path, out Image i);
            if (!ok)
            {
                throw new Exception($"SN_LoadImage: Failed to load image from path ${path}.");
            }
            return i;
        }

        public Func<string, Image> GetFunc => Execute;
    }
}
