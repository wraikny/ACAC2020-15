using System.Collections.Generic;
using System.Linq;

namespace ACAC2020_15.Shared
{
    sealed class AveragedLatency
    {
        private readonly Queue<float> latencies;

        public float LatencySecond { get; private set; }

        const int LantencyBufferCount = 8;

        public AveragedLatency()
        {
            latencies = new Queue<float>();
        }

        public void UpdateLatency(int latencyMs)
        {
            if (latencies.Count == LantencyBufferCount)
            {
                latencies.Dequeue();
            }

            latencies.Enqueue(Shared.Utils.MsToSec(latencyMs));
            LatencySecond = latencies.Average();
        }
    }
}
