using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class SnailFishNumber
    {
        public SnailFishNumber(SnailFishNumber left, SnailFishNumber right)
        {
            LeftNumber = left;
            RightNumber = right;

            LeftNumber.Parent = this;
            RightNumber.Parent = this;
        }

        public SnailFishNumber(string text, SnailFishNumber parent)
        {
            Parent = parent;

            var snailFishPattern = @"\[(?<allText>.+)\]";
            var snailfishMatch = Regex.Match(text, snailFishPattern);
            var insideText = snailfishMatch.Groups["allText"].Value;
            var middleCommaIndex = FindMiddleCommaIndex(insideText);

            var leftNumberText = insideText.Substring(0, middleCommaIndex);
            var rightNumberText = insideText.Substring(middleCommaIndex + 1);
            if (Regex.IsMatch(leftNumberText, snailFishPattern))
            {
                LeftNumber = new SnailFishNumber(leftNumberText, this);
            }
            else
            {
                LeftNumberValue = int.Parse(leftNumberText);
            }

            if (Regex.IsMatch(rightNumberText, snailFishPattern))
            {
                RightNumber = new SnailFishNumber(rightNumberText, this);
            }
            else
            {
                RightNumberValue = int.Parse(rightNumberText);
            }
        }

        private static int FindMiddleCommaIndex(string insideText)
        {
            var commaMatches = Regex.Matches(insideText, ",");
            var bracketMatches = Regex.Matches(insideText, @"[\[\]]");

            var bracketsByIndex = new Dictionary<int, string>();

            foreach (Match bracketMatch in bracketMatches)
            {
                bracketsByIndex[bracketMatch.Index] = bracketMatch.Value;
            }

            foreach (Match commaMatch in commaMatches)
            {
                var index = commaMatch.Index;

                var brackets = bracketsByIndex.Where(x => x.Key < index).ToList();

                if (brackets.Count(x => x.Value == "[") == brackets.Count(x => x.Value == "]"))
                {
                    return index;
                }
            }

            throw new System.Exception();
        }

        public bool HasExplodableChildren => Parent?.Parent?.Parent != null &&
            ((LeftNumber != null) || (RightNumber != null)) && LeftNumber?.HasExplodableChildren != true && RightNumber?.HasExplodableChildren != true;

        public bool HasSplittableChildren => LeftNumberValue >= 10 || RightNumberValue >= 10;
        public SnailFishNumber LeftNumber { get; set; }
        public int LeftNumberValue { get; set; }
        public SnailFishNumber Parent { get; set; }
        public SnailFishNumber RightNumber { get; set; }
        public int RightNumberValue { get; set; }

        public void ExplodeLeftmostChild()
        {
            if (LeftNumber != null)
            {
                Parent?.PropagateLeft(LeftNumber.LeftNumberValue, this);
                if (RightNumber != null)
                {
                    RightNumber.PropagateRight(LeftNumber.RightNumberValue, this);
                }
                else
                {
                    RightNumberValue += LeftNumber.RightNumberValue;
                }

                LeftNumber = null;
                LeftNumberValue = 0;
            }
            else if (RightNumber != null)
            {
                if (LeftNumber != null)
                {
                    LeftNumber.PropagateLeft(RightNumber.LeftNumberValue, this);
                }
                else
                {
                    LeftNumberValue += RightNumber.LeftNumberValue;
                }

                Parent.PropagateRight(RightNumber.RightNumberValue, this);

                RightNumber = null;
                RightNumberValue = 0;
            }
        }

        public void SplitLeftmostChild()
        {
            if (LeftNumberValue >= 10)
            {
                var leftValue = LeftNumberValue / 2;
                var rightValue = (int)(LeftNumberValue / 2.0 + 0.6);
                LeftNumber = new SnailFishNumber($"[{leftValue},{rightValue}]", this);
                LeftNumberValue = 0;
            }
            else if (RightNumberValue >= 10)
            {
                var leftValue = RightNumberValue / 2;
                var rightValue = (int)(RightNumberValue / 2.0 + 0.6);
                RightNumber = new SnailFishNumber($"[{leftValue},{rightValue}]", this);
                RightNumberValue = 0;
            }
        }

        private void PropagateLeft(int number, SnailFishNumber source)
        {
            if (source == Parent)
            {
                if (RightNumber != null)
                {
                    RightNumber.PropagateLeft(number, this);
                }
                else
                {
                    RightNumberValue += number;
                }
            }
            else if (source == LeftNumber)
            {
                Parent?.PropagateLeft(number, this);
            }
            else if (source == RightNumber)
            {
                if (LeftNumber != null)
                {
                    LeftNumber.PropagateLeft(number, this);
                }
                else
                {
                    LeftNumberValue += number;
                }
            }
        }

        private void PropagateRight(int number, SnailFishNumber source)
        {
            if (source == Parent)
            {
                if (LeftNumber != null)
                {
                    LeftNumber.PropagateRight(number, this);
                }
                else
                {
                    LeftNumberValue += number;
                }
            }
            else if (source == RightNumber)
            {
                Parent?.PropagateRight(number, this);
            }
            else if (source == LeftNumber)
            {
                if (RightNumber != null)
                {
                    RightNumber.PropagateRight(number, this);
                }
                else
                {
                    RightNumberValue += number;
                }
            }
        }

        public void ReduceIfNecessary()
        {
            bool reductionPerformed;
            do
            {
                reductionPerformed = LeftNumber?.CheckForExplosions() ?? false;
                if (!reductionPerformed)
                {
                    reductionPerformed = RightNumber?.CheckForExplosions() ?? false;
                }

                if (!reductionPerformed)
                {
                    reductionPerformed = LeftNumber?.CheckForSplits() ?? false;
                }

                if (!reductionPerformed)
                {
                    reductionPerformed = RightNumber?.CheckForSplits() ?? false;
                }
            }
            while (reductionPerformed);            
        }

        private bool CheckForSplits()
        {
            if (LeftNumber?.CheckForSplits() == true)
            {
                return true;
            }            
            else if (HasSplittableChildren)
            {
                SplitLeftmostChild();
                return true;
            }
            else if (RightNumber?.CheckForSplits() == true)
            {
                return true;
            }

            return false;
        }

        private bool CheckForExplosions()
        {
            if (HasExplodableChildren)
            {
                ExplodeLeftmostChild();
                return true;
            }

            return LeftNumber?.CheckForExplosions() == true || RightNumber?.CheckForExplosions() == true;
        }

        public long CalculateMagnitude()
        {
            long leftMagnitude, rightMagnitude;
            if (LeftNumber != null)
            {
                leftMagnitude = 3 * LeftNumber.CalculateMagnitude();
            }
            else
            {
                leftMagnitude = 3 * LeftNumberValue;
            }

            if (RightNumber != null)
            {
                rightMagnitude = 2 * RightNumber.CalculateMagnitude();
            }
            else
            {
                rightMagnitude = 2 * RightNumberValue;
            }

            return leftMagnitude + rightMagnitude;
        }
        public override string ToString()
        {
            return $"[{LeftNumber?.ToString() ?? LeftNumberValue.ToString()},{RightNumber?.ToString() ?? RightNumberValue.ToString()}]";
        }
    }
}