using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public static class Helpers
    {
        public static bool IsBetween(this int value, int low, int high)
        {
            return value >= low && value <= high;
        }

        public static bool IsBetween(this long value, long low, long high)
        {
            return value >= low && value <= high;
        }
    }
}
