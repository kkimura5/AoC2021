using System;
using System.Collections.Generic;

namespace AdventOfCode
{
    public static class Day10Methods
    {
        public static long ComputeCorruptScore(List<char> closesRequired)
        {
            long corruptScore = 0;
            foreach (var requiredClose in closesRequired)
            {
                corruptScore *= 5;
                switch (requiredClose)
                {
                    case ')':
                        corruptScore += 1;
                        break;

                    case ']':
                        corruptScore += 2;
                        break;

                    case '}':
                        corruptScore += 3;
                        break;

                    case '>':
                        corruptScore += 4;
                        break;
                }
            }

            return corruptScore;
        }

        public static char GetCloseParen(char character)
        {
            switch (character)
            {
                case '(':
                    return ')';

                case '[':
                    return ']';

                case '{':
                    return '}';

                case '<':
                    return '>';
            }

            throw new Exception();
        }

        public static int GetScore(char character)
        {
            switch (character)
            {
                case ')':
                    return 3;

                case ']':
                    return 57;

                case '}':
                    return 1197;

                case '>':
                    return 25137;
            }

            throw new Exception();
        }
    }
}