using System.Collections.Generic;
using System.Linq;

namespace REVUnit.Crlib
{
    public class Averager
    {
        private readonly Queue<decimal> samples;

        public Averager(int period)
        {
            Period = period;
            samples = new Queue<decimal>(period);
        }

        public decimal Alpha { get; } = 0.1m;

        public int Period { get; }

        public decimal CurrentMa => samples.Count != 0 ? samples.Average() : 0m;

        public decimal CurrentWma
        {
            get
            {
                return samples.DefaultIfEmpty().Aggregate((ema, nextQuote) => Alpha * nextQuote + (1m - Alpha) * ema);
            }
        }

        public void Push(decimal sample)
        {
            if (samples.Count == Period) samples.Dequeue();
            samples.Enqueue(sample);
        }

        public void ClearSample()
        {
            samples.Clear();
        }
    }
}