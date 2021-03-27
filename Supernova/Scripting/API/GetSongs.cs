using Supernova.Core;
using Supernova.Disk;
using Supernova.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supernova.Scripting.API
{
    [ExposeToLua("SN_GetSongs")]
    class GetSongs
    {
        public List<Folder> Execute()
        {
            if (SupernovaMain.SongFolders != null)
            {
                return SupernovaMain.SongFolders;
            } else
            {
                return null;
            } 
        }

        public Func<List<Folder>> GetFunc => Execute;
    }
}
