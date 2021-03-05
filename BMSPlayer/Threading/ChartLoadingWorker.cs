using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Supernova.BMS;

namespace Supernova.Threading
{
    class ChartLoaderThread
    {
        public delegate void DoneLoading(BMSChart ch);
        private DoneLoading callback;
        private string bmsPath;

        public ChartLoaderThread(string path, DoneLoading cb)
        {
            callback = cb;
            bmsPath = path;
        }

        public void ThreadProcess()
        {
            // This is where the actual threading goes
            var ch = BMSParser.ParseBMSChart(bmsPath);
            callback?.Invoke(ch);
        }
    }

    public class ChartLoadingWorker
    {
        public delegate void ChartLoadDoneCallback(BMSChart ch);
        public event ChartLoadDoneCallback OnChartDoneLoading;
        public Thread thread;

        public void StartLoadingChart(string path)
        {
            var clt = new ChartLoaderThread(path, (BMSChart c) =>
            {
                OnChartDoneLoading(c);
            });

            thread = new Thread(new ThreadStart(clt.ThreadProcess));
            thread.Start();
        }
    }
}
