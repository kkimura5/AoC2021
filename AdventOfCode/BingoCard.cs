using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class BingoCard
    {
        private const int INT_CardSize = 5;
        private Dictionary<int,bool> card;

        public BingoCard(List<int> numbers)
        {
            card = numbers.ToDictionary(x => x, x => false);
        }

        public void Mark(int number)
        {
            if (card.ContainsKey(number))
            {
                card[number] = true;
            }
        }

        public bool IsBingo()
        {
            return Enumerable.Range(0, INT_CardSize).Any(x => IsRowBingo(x) || IsColumnBingo(x));
        }

        private bool IsRowBingo(int row)
        {
            return card.ToList().Skip(INT_CardSize * row).Take(INT_CardSize).All(x => x.Value);
        }

        private bool IsColumnBingo(int col)
        {
            var indices = Enumerable.Range(0, INT_CardSize).Select(x => col + (x * INT_CardSize)).ToList();
            return card.ToList().Where((x, i) => indices.Contains(i)).All(x => x.Value);
        }

        public int SumUnmarkedValues()
        {
            return card.Where(x => !x.Value).Sum(x => x.Key);
        }

        public static List<BingoCard> SplitIntoBingoCards(List<string> lines, int numRows)
        {
            var bingoCards = new List<BingoCard>();
            while (lines.Any())
            {
                var numbers = new List<int>();
                var input = string.Join(" ", lines.Take(numRows));
                var matches = Regex.Matches(input, @"\d+");
                foreach (Match match in matches)
                {
                    numbers.Add(int.Parse(match.Value));
                }

                bingoCards.Add(new BingoCard(numbers));
                lines.RemoveRange(0, numRows);
            }

            return bingoCards;
        }
    }
}
