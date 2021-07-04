using System;

namespace PlywoodViolin.Monkey
{
    public class Random : IRandom
    {
        public decimal GetRandomValue()
        {
            return (decimal)new System.Random(Guid.NewGuid().GetHashCode()).NextDouble();
        }
    }
}
