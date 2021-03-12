using System.Collections.Generic;
using System.Linq;

namespace REVUnit.Crlib
{
    public class MovingAverage
    {
        private readonly Queue<decimal> _samples;

        public MovingAverage(int period)
        {
            Period = period;
            _samples = new Queue<decimal>(period);
        }

        public decimal Alpha { get; } = 0.1m;

        public int Period { get; }

        public decimal CurrentMovingAverage => _samples.Count != 0 ? _samples.Average() : 0m;

        public decimal CurrentWeightedMovingAverage
        {
            get
            {
                return _samples.DefaultIfEmpty().Aggregate((ema, nextQuote) => Alpha * nextQuote + (1m - Alpha) * ema);
            }
        }

        public void ClearSample()
        {
            _samples.Clear();
        }

        public void Push(decimal sample)
        {
            if (_samples.Count == Period) _samples.Dequeue();
            _samples.Enqueue(sample);
        }
    }
}