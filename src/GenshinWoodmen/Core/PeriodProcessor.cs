using System;
using System.Threading;

namespace GenshinWoodmen.Core
{
    internal class PeriodProcessor
    {
        public Thread PeriodThread;
        public Action PeriodWork;

        public PeriodProcessor(Action periodWork)
        {
            PeriodWork = periodWork;
            PeriodThread = new(GetPeriod)
            {
                IsBackground = true,
            };
            PeriodThread.Start();
        }

        public void GetPeriod()
        {
            while (true)
            {
                PeriodWork?.Invoke();
                Thread.Sleep(1500);
            }
        }
    }
}
