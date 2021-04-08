using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Supernova.Gameplay;
using Luminal.Logging;

namespace Supernova.Configuration
{
    class GameplayConfig
    {
        public static void Save()
        {
            var d = JsonSerializer.Serialize(GameplayOptions.Instance, typeof(GameplayOptions), new()
            {
                IncludeFields = true
            });
            Log.Debug(d);
        }
    }
}
