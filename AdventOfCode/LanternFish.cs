using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class LanternFish
    {
        public LanternFish(int count)
        {
            Count = count;
        }

        public int Count { get; private set; }
        public bool IsSpawning => Count < 0;
        public void Decrement()
        {
            Count--;
        }

        public void Reset()
        {
            Count = 6;
        }
    }
}
