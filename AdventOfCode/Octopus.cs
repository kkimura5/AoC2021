using System;
using System.Collections.Generic;
using System.Drawing;

namespace AdventOfCode
{
    internal class Octopus
    {
        private int value;

        public int X { get; private set; }
        public int Y { get; private set; }
        public bool IsFlashing { get; private set; }
        public bool HasFlashed { get; private set; }

        public Octopus(int v, int x, int y)
        {
            this.value = v;
            X = x;
            Y = y;
        }

        public void Increment()
        {
            if (HasFlashed)
            {
                return;
            }

            value++;
            if (value > 9)
            {
                value = 0;
                IsFlashing = true;
                HasFlashed = true;
            }
        }

        public void Respond()
        {
            IsFlashing = false;
        }

        public void Reset()
        {
            HasFlashed = false;
        }

        internal IEnumerable<Point> GetAdjacentPoints()
        {
            return new List<Point>
            {
                new Point(X - 1, Y-1),
                new Point(X + 1, Y+1),
                new Point(X - 1, Y+1),
                new Point(X + 1, Y-1),
                new Point(X - 1, Y),
                new Point(X + 1, Y),
                new Point(X, Y-1),
                new Point(X, Y+1),
            };
        }
    }
}