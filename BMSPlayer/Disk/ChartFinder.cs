using Supernova.BMS;
using Supernova.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Supernova.Disk
{
    public class ChartFinder
    {
        public static readonly string[] ACCEPTABLE_EXTENSIONS =
        {
            ".bms",
            ".bme"
        };

        public static Dictionary<string, Folder> FindCharts()
        {
            var paths = SNGlobal.Config.SongPaths;
            if (paths == null)
            {
                throw new Exception("Config.SongPaths is null.");
            }

            var folders = new Dictionary<string, Folder>();
            
            foreach (var p in paths)
            {
                var folderName = Path.GetFileName(p);
                var folder = new Folder()
                {
                    Name = folderName
                };

                var subdirs = Directory.GetDirectories(p);
                foreach (var sd in subdirs)
                {
                    var files = (from file in Directory.EnumerateFiles(sd)
                                 where Array.IndexOf(ACCEPTABLE_EXTENSIONS, Path.GetExtension(file)) != -1
                                 select file).ToList();
                    foreach (var l in files)
                    {
                        // So many loops
                        var chart = BMSParser.ParseBMSChart(l, true);
                        //Console.WriteLine($"Chart hash {chart.MD5_Hash} (that's {chart.title}) is going in {folder.Name}");
                        folder.Charts[chart.MD5_Hash] = chart;
                    }
                }

                folders[p] = folder;
            }

            return folders;
        }
    }
}
