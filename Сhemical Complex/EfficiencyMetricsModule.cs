using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Сhemical_Complex
{
    public class EfficiencyMetricsModule
    {
        private static long _initialMemory;
        private static long _finalMemory;
        public static long MemoryUsedBytes { get; private set; }

        public static void PrepareMemoryMeasurement()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            _initialMemory = GC.GetTotalMemory(true);
        }

        public static void FinalizeMemoryMeasurement()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            _finalMemory = GC.GetTotalMemory(true);
            MemoryUsedBytes = Math.Max(0, _finalMemory - _initialMemory);
        }
    }
}
