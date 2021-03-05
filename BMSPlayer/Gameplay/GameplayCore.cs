using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supernova.BMS;
using Supernova.Threading;

namespace Supernova.Gameplay
{
    public class GameplayCore
    {
        public BMSChart Chart;
        public float BPM;
        public float Beat = 0f;
        public float Position = 0f;

        public bool Started = false;

        public void LoadGameplay(string path)
        {
            var cw = new ChartLoadingWorker();
            cw.OnChartDoneLoading += _Start;
            cw.StartLoadingChart(path);
        }

        void _Start(BMSChart ch)
        {
            Chart = ch;
            BPM = Chart.initialBPM;

            Started = true;
        }

        public void UpdateEngine(float Delta)
        {
            if (!Started) return;
            Position += Delta;
        }
    }
}
