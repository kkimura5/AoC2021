using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public struct Game
    {
        public int Position1 { get; set; }
        public int Position2 { get; set; }
        public int Score1 { get; set; }
        public int Score2 { get; set; }

        public bool IsOver => Score1 >= 21 || Score2 >= 21;

        internal Game Copy()
        {
            return new Game()
            {
                Position1 = Position1,
                Position2 = Position2,
                Score1 = Score1,
                Score2 = Score2,
            };
        }
    }
}
