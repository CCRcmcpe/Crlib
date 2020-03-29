using System.Collections.Generic;
using System.Linq;

namespace REVUnit.Crlib
{
    public class Averager
    {
        private readonly Queue<decimal> _samples;

        public Averager(int period)
        {
            Period = period;
            _samples = new Queue<decimal>(period);
        }

        public decimal Alpha { get; } = 0.1m;

        public int Period { get; }

        public decimal CurrentMa => _samples.Count != 0 ? _samples.Average() : 0m;

        public decimal CurrentWma
        {
            get
            {
                return _samples.DefaultIfEmpty().Aggregate((ema, nextQuote) => Alpha * nextQuote + (1m - Alpha) * ema);
            }
        }

        public void Push(decimal sample)
        {
            if (_samples.Count == Period) _samples.Dequeue();
            _samples.Enqueue(sample);
        }

        public void ClearSample()
        {
            _samples.Clear();
        }
    }
}