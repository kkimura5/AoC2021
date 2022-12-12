using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    internal class Program
    {
        private static int ComputeOutputValue(Match match)
        {
            var inputNames = new List<string>()
            {
                "input1",
                "input2",
                "input3",
                "input4",
                "input5",
                "input6",
                "input7",
                "input8",
                "input9",
                "input10"
            };

            var inputs = inputNames.Select(x => match.Groups[x].Value);

            var digits = new Dictionary<int, string>();
            digits[1] = inputs.Single(x => x.Length == 2);
            digits[7] = inputs.Single(x => x.Length == 3);
            digits[8] = inputs.Single(x => x.Length == 7);
            digits[4] = inputs.Single(x => x.Length == 4);

            var twoThreeFive = inputs.Where(x => x.Length == 5).ToList();
            var sixNineZero = inputs.Where(x => x.Length == 6).ToList();

            digits[6] = sixNineZero.Single(x => !digits[1].All(c => x.Contains(c)));
            digits[9] = sixNineZero.Single(x => digits[4].All(c => x.Contains(c)));
            digits[0] = sixNineZero.Single(x => x != digits[6] && x != digits[9]);

            digits[3] = twoThreeFive.Single(x => digits[1].All(c => x.Contains(c)));
            digits[2] = twoThreeFive.Single(x => digits[4].Count(c => x.Contains(c)) == 2);
            digits[5] = twoThreeFive.Single(x => x != digits[2] && x != digits[3]);

            var outputValue = digits.Single(x => x.Value.Length == match.Groups["digit1"].Value.Length && x.Value.All(c => match.Groups["digit1"].Value.Contains(c))).Key * 1000;
            outputValue += digits.Single(x => x.Value.Length == match.Groups["digit2"].Value.Length && x.Value.All(c => match.Groups["digit2"].Value.Contains(c))).Key * 100;
            outputValue += digits.Single(x => x.Value.Length == match.Groups["digit3"].Value.Length && x.Value.All(c => match.Groups["digit3"].Value.Contains(c))).Key * 10;
            outputValue += digits.Single(x => x.Value.Length == match.Groups["digit4"].Value.Length && x.Value.All(c => match.Groups["digit4"].Value.Contains(c))).Key * 1;

            return outputValue;
        }

        private static int CountDuplicates(List<Point> allPoints)
        {
            var groupings = allPoints.GroupBy(x => x);
            var duplicatePoints = groupings.Where(x => x.ToList().Count() > 1).ToList();
            return duplicatePoints.Count;
        }

        private static int CountEasyDigits(Match match)
        {
            var digits = new List<string>();
            digits.Add(match.Groups["digit1"].Value);
            digits.Add(match.Groups["digit2"].Value);
            digits.Add(match.Groups["digit3"].Value);
            digits.Add(match.Groups["digit4"].Value);

            return digits.Count(x => x.Length == 3 || x.Length == 2 || x.Length == 7 || x.Length == 4);
        }

        private static List<LanternFish> CountFish(List<LanternFish> lanternFish, int numDays)
        {
            for (int i = 0; i < numDays; i++)
            {
                lanternFish.ForEach(x => x.Decrement());

                var spawningFish = lanternFish.Where(x => x.IsSpawning).ToList();
                foreach (var fish in spawningFish)
                {
                    fish.Reset();
                    lanternFish.Add(new LanternFish(8));
                }
            }

            return lanternFish;
        }


        private static int FindBasinSize(int x, int y, List<string> heightGrid)
        {
            var points = new List<Point>() { new Point(x, y) };
            var isEnd = false;
            while (!isEnd)
            {
                var startingPoints = points.ToList();
                foreach (var point in startingPoints)
                {
                    var pointsToCheck = new List<Point>
                    {
                        new Point(point.X+1, point.Y),
                        new Point(point.X-1, point.Y),
                        new Point(point.X, point.Y+1),
                        new Point(point.X, point.Y-1),
                    };

                    var pointsToAdd = pointsToCheck.Where(p => p.Y < heightGrid.Count && p.Y >= 0 && p.X >= 0 && p.X < heightGrid[y].Length).ToList();
                    points.AddRange(pointsToAdd.Where(p => heightGrid[p.Y][p.X] != '9' && !points.Contains(p)));
                }

                isEnd = points.Count == startingPoints.Count;
            }

            return points.Count;
        }

        private static int GetNumValuesGreaterThanPrevious(List<int> numbers)
        {
            var list1 = numbers.Skip(1).ToList();
            var list2 = numbers.Take(numbers.Count - 1).ToList();

            var count = list1.Select((x, i) => x > list2[i]).Count(x => x);
            return count;
        }

        private static bool IsMostCommonBitOne(List<int> numbers, int index)
        {
            var numOnes = numbers.Where(x => ((x >> index) & 1) > 0).Count();
            var numZeros = numbers.Where(x => ((x >> index) & 1) == 0).Count();
            return numOnes >= numZeros;
        }

        private static void Main(string[] args)
        {
            RunDay1();
            RunDay2();
            RunDay3();
            RunDay4();
            RunDay5();
            // RunDay6();
            // RunDay7();
            RunDay8();
            RunDay9();
            RunDay10();
            RunDay11();
            //RunDay12();
            RunDay13();
            RunDay14();
            //RunDay15();
            // RunDay16();
            // RunDay17();
            //RunDay18();
            //RunDay19();
            //RunDay20();
            //RunDay21();
            RunDay22();

            Console.ReadKey();
        }

        private static void RunDay19()
        {
            var lines = File.ReadAllLines("InputData\\input19.txt").ToList();
        }

        private static void RunDay22()
        {
            var lines = File.ReadAllLines("InputData\\input22.txt").ToList();
            var completedSteps = new List<RebootStep>();
            long cubesOn = 0;

            foreach (var line in lines)
            {
                var newStep = new RebootStep(line);

                if (newStep.IsInRange(50))
                {
                    long numIntersectingPoints = GetNumIntersectingPoints(completedSteps, newStep);

                    if (newStep.IsOn)
                    {
                        cubesOn += newStep.NumCubes - numIntersectingPoints;
                    }
                    else
                    {
                        cubesOn -= numIntersectingPoints;
                    }

                    completedSteps.Add(newStep);
                }

            }

            Console.WriteLine($"Day 22 part 1: {cubesOn}");
        }

        private static long GetNumIntersectingPoints(List<RebootStep> completedSteps, RebootStep newStep)
        {
            long numIntersectingPoints = 0;
            var intersections = new List<RebootStep>();
            foreach (var completedStep in completedSteps)
            {
                var intersection = newStep.GetIntersection(completedStep);

                long numNewIntersectingPoints = intersection.NumCubes;
                if (!intersection.IsNull)
                {
                    if (intersections.Any())
                    {
                        numNewIntersectingPoints -= GetNumIntersectingPoints(intersections, intersection);
                    }

                    intersections.Add(intersection);
                }
                    

                if (completedStep.IsOn)
                {
                    numIntersectingPoints += numNewIntersectingPoints;
                }
                else
                {
                    numIntersectingPoints -= numNewIntersectingPoints;
                }
            }

            return numIntersectingPoints;
        }

        private static void RunDay21()
        {
            var positions = new List<int>() { 1, 10 };
            RunDeterministicDice(positions.ToList());
            RunDiracDice(positions.ToList());
        }

        private static void RunDiracDice(List<int> positions)
        {
            var wonGames = new List<long> { 0, 0 };
            var activeGames = new Dictionary<Game, long>();
            activeGames[new Game() { Position1 = positions[0], Position2 = positions[1] }] = 1;
            var turn = 0;
            while (activeGames.Any(x => x.Value != 0))
            {
                var currentGames = activeGames.Where(x => x.Value != 0).ToDictionary(x => x.Key, x => x.Value);
                foreach (var currentGame in currentGames)
                {
                    var game = currentGame.Key;
                    var newGames = CreateNewGames(game, turn);
                    wonGames[turn] += newGames.Count(x => x.IsOver) * currentGame.Value;

                    foreach (var newGame in newGames.Where(x => !x.IsOver))
                    {
                        if (!activeGames.ContainsKey(newGame))
                        {
                            activeGames[newGame] = 0;
                        }

                        activeGames[newGame] += currentGame.Value;
                    }

                    activeGames[game] -= currentGame.Value;
                }

                turn++;
                turn %= 2;
            }

            Console.WriteLine($"Day 21 Part 2: Player 1 wins {wonGames[0]}, Player 2 wins {wonGames[1]}");
        }

        private static List<Game> CreateNewGames(Game game, int turn)
        {
            var newGames = new List<Game>();

            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    for (int k = 1; k <= 3; k++)
                    {
                        var newGame = game.Copy();
                        if (turn == 0)
                        {
                            newGame.Position1 += i + j + k;
                            while (newGame.Position1 > 10)
                            {
                                newGame.Position1 -= 10;
                            }
                            
                            newGame.Score1 += newGame.Position1;
                        }
                        else
                        {
                            newGame.Position2 += i + j + k;
                            while (newGame.Position2 > 10)
                            {
                                newGame.Position2 -= 10;
                            }

                            newGame.Score2 += newGame.Position2;
                        };

                        newGames.Add(newGame);
                    }
                }
            }

            return newGames;
        }

        private static void RunDeterministicDice(List<int> positions)
        {
            var scores = new List<int>() { 0, 0 };
            var count = 0;
            var dieValue = 1;
            var turn = 0;
            while (scores.All(x => x < 1000))
            {
                positions[turn] += 3 * dieValue + 3;
                while (positions[turn] > 10)
                {
                    positions[turn] -= 10;
                }

                scores[turn] += positions[turn];
                dieValue += 3;
                count += 3;
                turn++;
                turn %= 2;
            }

            Console.WriteLine($"Day 21 part 1: {scores.Min() * count}");
        }

        private static void RunDay20()
        {
            var lines = File.ReadAllLines("InputData\\input20.txt").ToList();

            var algorithm = lines.First();
            var image = lines.Skip(2).ToList();

            for (int i = 0; i < 25; i++)
            {
                image = Enhance(algorithm, image.ToList(),'.');
                image = Enhance(algorithm, image.ToList(),'#');
            }

            var output = string.Join(Environment.NewLine, image);
            Console.WriteLine($"Day 20: {string.Join("", image).Count(x => x == '#')}");
        }

        private static List<string> Enhance(string algorithm, List<string> image, char paddingCharacter)
        {
            var paddedImage = image.Select(x => $"{paddingCharacter}{paddingCharacter}{paddingCharacter}{x}{paddingCharacter}{paddingCharacter}{paddingCharacter}").ToList();
            var dots = string.Join("", Enumerable.Range(0, paddedImage.First().Length).Select(x => $"{paddingCharacter}"));
            var newImage = new List<string>();

            paddedImage.Insert(0, dots);
            paddedImage.Insert(0, dots);
            paddedImage.Insert(0, dots);
            paddedImage.Add(dots);
            paddedImage.Add(dots);
            paddedImage.Add(dots);


            for (int j = 0; j < paddedImage.Count - 3; j++)
            {
                var characters = new List<char>();

                for (int i = 0; i < paddedImage.First().Length-3; i++)
                {
                    var encodedValue = string.Join("", paddedImage.Skip(j).Take(3).Select(x => x.Substring(i, 3)));
                    var value = (int)encodedValue.Select((c, index) => c == '#' ? Math.Pow(2, 8 - index) : 0).Sum();
                    characters.Add(algorithm[value]);
                }

                newImage.Add(string.Join("", characters.Select(c => $"{c}")));
            }

            return newImage;
        }

        private static void RunDay18()
        {
            var lines = File.ReadAllLines("InputData\\input18.txt");
            ComputeTotalMagnitude(lines);
            ComputeMaxMagnitude(lines);
        }

        private static void ComputeMaxMagnitude(string[] lines)
        {
            var maxMagnitude = 0L;
            for (int i = 0; i < lines.Count(); i++)
            {
                var firstLine = lines[i];
                for (int j = i; j < lines.Count(); j++)
                {
                    var secondLine = lines[j];
                    var sum1 = new SnailFishNumber($"[{firstLine},{secondLine}]", null);
                    var sum2 = new SnailFishNumber($"[{secondLine},{firstLine}]", null);

                    sum1.ReduceIfNecessary();
                    sum2.ReduceIfNecessary();

                    maxMagnitude = new List<long>() { maxMagnitude, sum1.CalculateMagnitude(), sum2.CalculateMagnitude() }.Max();
                }
            }

            Console.WriteLine($"Day 18 part 2: max magnitude = {maxMagnitude}");
        }

        private static void ComputeTotalMagnitude(string[] lines)
        {
            var topNumber = new SnailFishNumber(lines.First(), null);
            topNumber.ReduceIfNecessary();

            foreach (var number in lines.Skip(1))
            {
                var newNumber = new SnailFishNumber(number, null);
                topNumber = new SnailFishNumber($"[{topNumber},{newNumber}]", null);
                topNumber.ReduceIfNecessary();
            }

            var magnitude = topNumber.CalculateMagnitude();

            Console.WriteLine($"Day 18 part 1: magnitude = {magnitude}");
        }

        private static void RunDay17()
        {
            var lines = File.ReadAllLines("InputData\\input17.txt");
            var pattern = @"target area: x=(?<xMin>\d+)..(?<xMax>\d+), y=(?<yMin>-\d+)..(?<yMax>-\d+)";
            var match = Regex.Match(lines.First(), pattern);
            var xMin = int.Parse(match.Groups["xMin"].Value);
            var xMax = int.Parse(match.Groups["xMax"].Value);
            var yMin = int.Parse(match.Groups["yMin"].Value);
            var yMax = int.Parse(match.Groups["yMax"].Value);

            long yVelocity = yMin-1;
            var count = 0;
            while (yVelocity < Math.Abs(20 * yMax))
            {
                yVelocity++;
                if (CanProbeHitTargetArea(yVelocity, yMin, yMax))
                {
                    var possibleSteps = GetPossibleSteps(yVelocity, yMin, yMax);
                    var possibleXVelocities = GetXVelocities(possibleSteps, xMin, xMax);
                    count += possibleXVelocities.Count;
                    var maxHeight = GetMaxHeight(yVelocity);
                    Console.WriteLine($"Day 17: Can hit at yVelocity = {yVelocity}; max height {maxHeight}, possible x velocities {string.Join(",",possibleXVelocities)} total count = {count}");
                }
            }
            
        }

        private static List<int> GetXVelocities(List<int> possibleSteps, int xMin, int xMax)
        {
            var possibleXVelocities = new List<int>();
            var startingXVelocity = xMax;

            while (startingXVelocity > 0)
            {
                var distance = 0;
                var step = 0;
                var xVelocity = startingXVelocity;
                while (xVelocity > 0)
                {
                    step++;
                    distance += xVelocity;
                    if(possibleSteps.Contains(step) && distance <= xMax && distance >= xMin)
                    {
                        possibleXVelocities.Add(startingXVelocity);
                    }

                    xVelocity--;
                }

                if (possibleSteps.Any(x => x >= step) && distance <= xMax && distance >= xMin)
                {
                    possibleXVelocities.Add(startingXVelocity);
                }

                startingXVelocity--;
            }
            

            return possibleXVelocities.Distinct().ToList();
        }

        private static List<int> GetPossibleSteps(long yVelocity, int yMin, int yMax)
        {
            var height = 0L;
            var step = 0;
            var possibleSteps = new List<int>();
            while (height >= yMin)
            {
                step++;
                height += yVelocity--;

                if (height >= yMin && height <= yMax)
                {
                    possibleSteps.Add(step);
                }
            }

            return possibleSteps;
        }

        private static long GetMaxHeight(long yVelocity)
        {
            if (yVelocity <= 0)
            {
                return 0;
            }
            else
            {
                return Enumerable.Range(1, (int)yVelocity).Sum();
            }
        }

        private static bool CanProbeHitTargetArea(long yVelocity, int minY, int maxY)
        {
            var canHit = false;
            var height = 0L;
            while(height >= minY)
            {
                height += yVelocity;
                yVelocity--;

                canHit = canHit || (height >= minY && height <= maxY); 
            }

            return canHit;
        }

        private static void RunDay16()
        {
            var lines = File.ReadAllLines("InputData\\input16.txt");
            var packet = Packet.FromText(new PacketTracker(lines.First()));
            var versions = packet.GetVersions();
            var value = packet.GetPacketValue();
            Console.WriteLine($"Day 16 part 1: {versions.Sum()}");
            Console.WriteLine($"Day 16 part 2: {value}");
        }

        private static void RunDay15()
        {
            var lines = File.ReadAllLines("InputData\\input15.txt").Select(l => l.Select(c => int.Parse($"{c}")).ToList()).ToList();
            var numRepeats = 5;
            var outputLines = GetOutputLines(lines, numRepeats);
            var size = outputLines.Count;

            var effort = new int[size, size];
            effort[0, 0] = 0;

            for (int i = 1; i < size * 2; i++)
            {
                var points = new List<Point>();
                for (int j = 0; j <= i; j++)
                {
                    if ((i - j) < size && j < size)
                    {
                        points.Add(new Point(j, i - j));
                    }
                }

                if (i > 2)
                {
                    RecalculatePreviousRisks(outputLines, size, effort, i);
                }

                foreach (var point in points)
                {
                    effort[point.X, point.Y] = GetLowestRisk(effort, outputLines, point);
                }
            }

            Console.WriteLine($"Day 15: {effort[size - 1, size - 1]}");
        }

        private static void RecalculatePreviousRisks(List<List<int>> outputLines, int size, int[,] effort, int i)
        {
            var count = i / 20 + 1;
            for (int n = count; n >= 1; n--)
            {
                var k = i - (2 * n);
                var l = i - (2 * n - 1);

                for (int j = 0; j <= k; j++)
                {
                    if ((k - j) < size && j < size)
                    {
                        var point = new Point(j, k - j);
                        effort[point.X, point.Y] = GetLowestRisk(effort, outputLines, point, true);
                    }
                }

                for (int j = 0; j <= l; j++)
                {
                    if ((l - j) < size && j < size)
                    {
                        var point = new Point(j, l - j);
                        effort[point.X, point.Y] = GetLowestRisk(effort, outputLines, point);
                    }
                }
            }
        }

        private static List<List<int>> GetOutputLines(List<List<int>> lines, int numRepeats)
        {
            var outputLines = new List<List<int>>();
            for (int i = 0; i < numRepeats; i++)
            {
                var newLines = new List<List<int>>();
                for (int j = 0; j < numRepeats; j++)
                {
                    foreach (var line in lines)
                    {
                        if (newLines.Count <= lines.IndexOf(line))
                        {
                            newLines.Add(new List<int>());
                        }

                        newLines[lines.IndexOf(line)].AddRange(line.Select(d => GetOutputValue(d, i, j)).ToList());
                    }
                }

                outputLines.AddRange(newLines);
            }

            return outputLines;
        }

        private static int GetOutputValue(int d, int i, int j)
        {
            var output = d + i + j;

            while (output > 9)
            {
                output -= 9;
            }

            return output;
        }

        private static int GetLowestRisk(int[,] effort, List<List<int>> lines, Point point, bool otherDirection = false)
        {
            var x = point.X;
            var y = point.Y;

            var newEffort = lines[y][x];
            var previousEffort = new List<int>();

            if (x -1 >= 0)
            {
                previousEffort.Add(effort[x - 1, y]);
            }

            if (y-1 >= 0)
            {
                previousEffort.Add(effort[x, y - 1]);
            }            

            if (otherDirection)
            {
                if (x + 1 < lines.Count)
                {
                    previousEffort.Add(effort[x + 1, y]);
                }

                if (y + 1 < lines.Count)
                {
                    previousEffort.Add(effort[x, y + 1]);
                }
            }

            return previousEffort.Min() + newEffort;
        }

        private static List<Tuple<Point, long>> MoveSteps(List<string> lines, Point startPoint, int numMoves)
        {
            var riskTrackers = new List<Tuple<Point, long>>();
            riskTrackers.Add(new Tuple<Point, long>(startPoint, 0));

            for (int i = 0; i < numMoves; i++)
            {
                var newTrackers = new List<Tuple<Point, long>>();
                foreach (var tracker in riskTrackers)
                {
                    if (tracker.Item1.X + 1 < lines.First().Length)
                    {
                        var newTracker = new Tuple<Point, long>(new Point(tracker.Item1.X + 1, tracker.Item1.Y), tracker.Item2 + int.Parse($"{lines[tracker.Item1.Y][tracker.Item1.X + 1]}"));
                        newTrackers.Add(newTracker);
                    }

                    if (tracker.Item1.Y + 1 < lines.Count)
                    {
                        var newTracker2 = new Tuple<Point, long>(new Point(tracker.Item1.X, tracker.Item1.Y + 1),
                            tracker.Item2 + int.Parse($"{lines[tracker.Item1.Y + 1][tracker.Item1.X]}"));
                        newTrackers.Add(newTracker2);
                    }
                }

                riskTrackers = newTrackers;
            }

            return riskTrackers;
        }

        private static void RunDay14()
        {
            var lines = File.ReadAllLines("InputData\\input14.txt").ToList();

            var polymer = lines.First();

            var rules = lines.Skip(2).Select(x => new KeyValuePair<string, string>(x.Substring(0, 2), $"{x.Trim().Last()}")).ToDictionary(x => x.Key, x => x.Value);

            var pairCounts = new Dictionary<string, long>();
            for (int j = 0; j < polymer.Length - 1; j++)
            {
                var pair = polymer.Substring(j,2);
                if (!pairCounts.ContainsKey(pair))
                {
                    pairCounts[pair] = 0;
                }

                pairCounts[pair]++;
            }

            var characterCounts = polymer.GroupBy(x => x).ToDictionary(x => x.Key, x => ((long)x.Count()));

            for (int i = 0; i < 40; i++)
            {
                var newPairCounts = new Dictionary<string, long>();
                foreach (var countByPair in pairCounts)
                {
                    var pair = countByPair.Key;

                    if (rules.ContainsKey(pair))
                    {
                        var stringToInsert = rules[pair];

                        var newPair1 = $"{pair[0]}{stringToInsert}";
                        var newPair2 = $"{stringToInsert}{pair[1]}";

                        if (!newPairCounts.ContainsKey(newPair1))
                        {
                            newPairCounts[newPair1] = 0;
                        }
                        if (!newPairCounts.ContainsKey(newPair2))
                        {
                            newPairCounts[newPair2] = 0;
                        }

                        newPairCounts[newPair1] += countByPair.Value;
                        newPairCounts[newPair2] += countByPair.Value;

                        if (!characterCounts.ContainsKey(stringToInsert[0]))
                        {
                            characterCounts[stringToInsert[0]] = 0;
                        }

                        characterCounts[stringToInsert[0]] += countByPair.Value;
                    }
                }

                pairCounts = newPairCounts;
            }

            long minCount = characterCounts.Min( x=> x.Value);
            long maxCount = characterCounts.Max(x => x.Value);

            Console.WriteLine($"Day 14: {maxCount - minCount}");
        }

        private static void RunDay13()
        {
            var lines = File.ReadAllLines("InputData\\input13.txt").ToList();
            var pattern1 = @"(?<X>\d+),(?<Y>\d+)";
            var pattern2 = @"fold along (?<axis>[yx])=(?<value>\d+)";
            var dots = new List<Point>();
            var folds = new List<Tuple<string, int>>();
            foreach (var line in lines)
            {
                var match1 = Regex.Match(line, pattern1);
                var match2 = Regex.Match(line, pattern2);
                if (match1.Success)
                {
                    dots.Add(new Point(int.Parse(match1.Groups["X"].Value), int.Parse(match1.Groups["Y"].Value)));
                }
                else if (match2.Success)
                {
                    folds.Add(new Tuple<string, int>(match2.Groups["axis"].Value, int.Parse(match2.Groups["value"].Value)));
                }
            }

            foreach (var fold in folds)
            {
                if (fold.Item1 == "x")
                {
                    var dotsToRemove = dots.Where(d => d.X > fold.Item2).ToList();
                    dots = dots.Except(dotsToRemove).ToList();
                    var newDots = dotsToRemove.Select(d => new Point(2 * fold.Item2 - d.X, d.Y)).ToList();
                    dots.AddRange(newDots.Where(d => !dots.Contains(d)));
                }
                else
                {
                    var dotsToRemove = dots.Where(d => d.Y > fold.Item2).ToList();
                    dots = dots.Except(dotsToRemove).ToList();
                    var newDots = dotsToRemove.Select(d => new Point(d.X, 2 * fold.Item2 - d.Y)).ToList();
                    dots.AddRange(newDots.Where(d => !dots.Contains(d)));
                }

                Console.WriteLine($"Day 13: after fold {fold.Item1}={fold.Item2}, now have {dots.Count} dots");
            }

            var maxX = dots.Max(d => d.X);
            var maxY = dots.Max(d => d.Y);

            for (int y = 0; y <= maxY; y++)
            {
                for (int x = 0; x <= maxX; x++)
                {
                    if (dots.Contains(new Point(x, y)))
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }

                Console.WriteLine();
            }
        }
        private static void RunDay12()
        {
            var lines = File.ReadAllLines("InputData\\input12.txt").ToList();
            var caves = Cave.CreateFromText(lines);

            var paths = FindPathsToEnd(caves);
            Console.WriteLine($"Day 12 part 1: there are {paths} possible paths");
            var paths2 = FindPathsToEnd(caves, true);

            Console.WriteLine($"Day 12 part 2: there are {paths2} possible paths");
        }

        private static int FindPathsToEnd(List<Cave> caves, bool allowSecondVisit = false)
        {
            var start = caves.Single(x => x.IsStart);

            var paths = start.LinkedCaves.Select(x => new List<Cave> { start, x }).ToList();
            var finishedPathCount = 0;
            while (paths.Any())
            {
                var unfinishedPaths = paths.ToList();
                foreach (var path in unfinishedPaths)
                {
                    paths.Remove(path);
                    var newPaths = path.Last().LinkedCaves.Select(x => path.Concat(new List<Cave>() { x }).ToList()).ToList();

                    if (allowSecondVisit)
                    {
                        paths.AddRange(newPaths.Where(IsPathValid));
                    }
                    else
                    {
                        paths.AddRange(newPaths.Where(p => !p.Any(c => !c.IsLargeCave && p.Count(x => x.Name == c.Name) > 1)));
                    }
                }

                finishedPathCount += paths.Count(x => x.Last().IsEnd);
                paths.RemoveAll(x => x.Last().IsEnd);
            }

            return finishedPathCount;
        }

        private static bool IsPathValid(List<Cave> path)
        {
            var smallCavesDoubleVisits = path.Where(c => !c.IsLargeCave && !c.IsStart && path.Count(x => x.Name == c.Name) == 2).Select(x=> x.Name).Distinct().ToList();
            var doubleVisitValid = smallCavesDoubleVisits.Count() <= 1;
            var otherVisitsValid = !path.Except(path.Where(x => smallCavesDoubleVisits.Contains(x.Name))).Any(c => !c.IsLargeCave && path.Count(x => x.Name == c.Name) > 1);
            return doubleVisitValid && otherVisitsValid;
        }

        private static void RunDay11()
        {
            var lines = File.ReadAllLines("InputData\\input11.txt").ToList();
            var octopi = lines.SelectMany((l, y) => l.Select((c, x) => new Octopus(int.Parse($"{c}"), x, y))).ToList();

            var numFlashes = 0;
            int i = 0;
            while (true)
            {
                octopi.ForEach(x => x.Increment());

                while (octopi.Any(x => x.IsFlashing))
                {
                    var flashingOctopi = octopi.Where(x => x.IsFlashing).ToList();
                    numFlashes += flashingOctopi.Count();
                    var allPoints = flashingOctopi.SelectMany(x => x.GetAdjacentPoints()).ToList();
                    flashingOctopi.ForEach(x => x.Respond());
                    var pointGroups = allPoints.Where(p => p.X >= 0 && p.Y >= 0 && p.X <= octopi.Max(o => o.X) && p.Y <= octopi.Max(o => o.Y)).GroupBy(x => x);

                    foreach (var pointGroup in pointGroups)
                    {
                        pointGroup.ToList().ForEach(x => octopi.Single(o => o.X == pointGroup.Key.X && o.Y == pointGroup.Key.Y).Increment());
                    }
                }

                if (i == 99)
                {
                    Console.WriteLine($"Day 11 part 1: there were {numFlashes} flashes");
                }

                if (octopi.All(x => x.HasFlashed))
                {
                    Console.WriteLine($"Day 11 part 2: all octopi flashed after day {i + 1}");
                    break;
                }

                octopi.Where(x => x.HasFlashed).ToList().ForEach(x => x.Reset());
                i++;
            }
        }

        private static void RunDay10()
        {
            var lines = File.ReadAllLines("InputData\\input10.txt");
            var score = 0;
            var corruptScores = new List<long>();
            foreach (var line in lines)
            {
                var isCorrupt = false;
                var closesRequired = new List<char>();
                foreach (var character in line)
                {
                    switch (character)
                    {
                        case '(':
                        case '[':
                        case '{':
                        case '<':
                            closesRequired.Insert(0, Day10Methods.GetCloseParen(character));
                            break;

                        case ')':
                        case ']':
                        case '}':
                        case '>':
                            var expectedClose = closesRequired[0];
                            closesRequired.RemoveAt(0);
                            if (expectedClose != character)
                            {
                                score += Day10Methods.GetScore(character);
                                isCorrupt = true;
                            }
                            break;
                    }

                    if (isCorrupt)
                    {
                        break;
                    }
                }

                if (!isCorrupt)
                {
                    corruptScores.Add(Day10Methods.ComputeCorruptScore(closesRequired));
                }
            }

            Console.WriteLine($"Day 10 Part 1 score: {score}");
            var list = corruptScores.OrderBy(x => x).ToList();
            Console.WriteLine($"Day 10 Part 2 score: {list[corruptScores.Count / 2]}");
        }



        private static void RunDay1()
        {
            var lines = File.ReadAllLines("InputData\\input1.txt");

            var numbers = lines.Select(x => int.Parse(x)).ToList();
            var sums = numbers.Skip(2).Select((x, i) => numbers.Skip(i).Take(3).Sum()).ToList();

            var count1 = GetNumValuesGreaterThanPrevious(numbers);
            var count2 = GetNumValuesGreaterThanPrevious(sums);

            Console.WriteLine($"Day1 Part 1 count: {count1}");
            Console.WriteLine($"Day1 Part 2 count: {count2}");
        }

        private static void RunDay2()
        {
            var lines = File.ReadAllLines("InputData\\input2.txt");

            var pattern = @"(?<direction>\w+) (?<value>\d+)";
            var matches = lines.Select(x => Regex.Match(x, pattern)).ToList();

            var forwardValue = matches.Where(x => x.Groups["direction"].Value == "forward").Select(x => int.Parse(x.Groups["value"].Value)).Sum();
            var upValue = matches.Where(x => x.Groups["direction"].Value == "up").Select(x => int.Parse(x.Groups["value"].Value)).Sum();
            var downValue = matches.Where(x => x.Groups["direction"].Value == "down").Select(x => int.Parse(x.Groups["value"].Value)).Sum();
            var aim = 0;
            var depth = 0;
            foreach (var match in matches)
            {
                var value = int.Parse(match.Groups["value"].Value);
                switch (match.Groups["direction"].Value)
                {
                    case "forward":
                        depth += value * aim;
                        break;

                    case "up":
                        aim -= value;
                        break;

                    case "down":
                        aim += value;
                        break;
                }
            }

            Console.WriteLine($"Day 2 Part 1 values: Forward {forwardValue}, depth {downValue - upValue}. Product = {forwardValue * (downValue - upValue)}");
            Console.WriteLine($"Day 2 Part 2 values: Forward {forwardValue}, depth {depth}. Product = {forwardValue * depth}");
        }

        private static void RunDay3()
        {
            var lines = File.ReadAllLines("InputData\\input3.txt");
            var allNumbers = lines.Select(x => Convert.ToInt32(x, 2)).ToList();
            var numBits = lines.First().Length;
            var gammaRate = 0;
            var epsilonRate = 0;
            var oxygenGeneratorList = allNumbers.ToList();
            var co2ScrubberList = allNumbers.ToList();

            for (int i = numBits - 1; i >= 0; i--)
            {
                var isCommonBitOne = IsMostCommonBitOne(allNumbers, i);
                gammaRate |= isCommonBitOne ? 1 << i : 0;
                epsilonRate |= isCommonBitOne ? 0 : 1 << i;

                if (oxygenGeneratorList.Count > 1)
                {
                    isCommonBitOne = IsMostCommonBitOne(oxygenGeneratorList, i);
                    oxygenGeneratorList = oxygenGeneratorList.Where(x => ((x >> i) & 1) == Convert.ToInt32(isCommonBitOne)).ToList();
                }

                if (co2ScrubberList.Count > 1)
                {
                    isCommonBitOne = IsMostCommonBitOne(co2ScrubberList, i);
                    co2ScrubberList = co2ScrubberList.Where(x => ((x >> i) & 1) == Convert.ToInt32(!isCommonBitOne)).ToList();
                }
            }

            Console.WriteLine($"Day 3 Part 1: Gamma {gammaRate}, Epsilon {epsilonRate}. Product {gammaRate * epsilonRate}");
            Console.WriteLine($"Day 3 Part 2: Oxygen {oxygenGeneratorList.First()}, CO2 {co2ScrubberList.First()}. Product {oxygenGeneratorList.First() * co2ScrubberList.First()}");
        }

        private static void RunDay4()
        {
            var lines = File.ReadAllLines("InputData\\input4.txt");
            var bingoNumbers = lines.First().Split(',').Select(x => int.Parse(x)).ToList();

            var bingoCards = BingoCard.SplitIntoBingoCards(lines.Skip(1).Where(x => !string.IsNullOrEmpty(x.Trim())).ToList(), 5);
            var winningCardCount = 0;
            foreach (var bingoNumber in bingoNumbers)
            {
                bingoCards.ForEach(x => x.Mark(bingoNumber));

                if (bingoCards.Any(x => x.IsBingo()))
                {
                    var winningCards = bingoCards.Where(x => x.IsBingo()).ToList();
                    foreach (var winningCard in winningCards)
                    {
                        var sumUnmarkedValues = winningCard.SumUnmarkedValues();
                        Console.WriteLine($"Day 4: winning card {++winningCardCount}, last number: {bingoNumber}; unmarked value sum: {sumUnmarkedValues}; product {bingoNumber * sumUnmarkedValues}");
                        bingoCards.Remove(winningCard);
                    }
                }
            }
        }

        private static void RunDay5()
        {
            var textLines = File.ReadAllLines("InputData\\input5.txt");
            var lines = textLines.Select(x => Line.Create(x)).ToList();
            var pointsFromNonDiagonalLines = lines.Where(x => x.IsVertical || x.IsHorizontal)
                                        .SelectMany(x => x.GetPoints()).ToList();

            Console.WriteLine($"Day 5 part 1: total duplicates: {CountDuplicates(pointsFromNonDiagonalLines)}");

            var allPoints = lines.SelectMany(x => x.GetPoints()).ToList();
            Console.WriteLine($"Day 5 part 2: total duplicates: {CountDuplicates(allPoints)}");
        }

        private static void RunDay6()
        {
            var lanternFish = File.ReadAllLines("InputData\\input6.txt").First().Split(',').Select(x => new LanternFish(int.Parse(x))).ToList();
            long count = 0;
            var cache = new Dictionary<int, int>();
            foreach (var fish in lanternFish)
            {
                var lanternFish1 = CountFish(new List<LanternFish> { fish }, 128);
                foreach (var fish2 in lanternFish1)
                {
                    var firstCount = fish2.Count;
                    if (!cache.ContainsKey(firstCount))
                    {
                        cache[firstCount] = CountFish(new List<LanternFish> { fish2 }, 128).Count;
                    }

                    count += cache[firstCount];
                }
            }

            Console.WriteLine($"Day 6 part 2: total num fish {count}");
        }

        private static void RunDay7()
        {
            var numbers = File.ReadAllLines("InputData\\input7.txt").First().Split(',').Select(x => int.Parse(x)).ToList();
            var max = numbers.Max();
            var min = numbers.Min();
            var currentMin1 = int.MaxValue;
            var currentMin2 = int.MaxValue;
            foreach (var value in Enumerable.Range(min, max - min + 1))
            {
                var sum = numbers.Select(x => Math.Abs(x - value)).Sum();

                if (currentMin1 > sum)
                {
                    currentMin1 = sum;
                }

                var sum2 = numbers.Select(x => Math.Abs(x - value)).Select(x => Enumerable.Range(1, x).Sum()).Sum();
                if (currentMin2 > sum2)
                {
                    currentMin2 = sum2;
                }
            }

            Console.WriteLine($"Day 7 part 1: min fuel = {currentMin1}");
            Console.WriteLine($"Day 7 part 2: min fuel = {currentMin2}");
        }

        private static void RunDay8()
        {
            var lines = File.ReadAllLines("InputData\\input8.txt");
            var pattern = @"(?<input1>\w+) (?<input2>\w+) (?<input3>\w+) (?<input4>\w+) (?<input5>\w+) (?<input6>\w+) (?<input7>\w+) (?<input8>\w+) (?<input9>\w+) (?<input10>\w+) \| (?<digit1>\w+) (?<digit2>\w+) (?<digit3>\w+) (?<digit4>\w+)";
            var count = 0;
            var outputValueCount = 0;
            foreach (var line in lines)
            {
                var match = Regex.Match(line, pattern);
                count += CountEasyDigits(match);
                outputValueCount += ComputeOutputValue(match);
            }

            Console.WriteLine($"Day 8 part 1: num 1,4,7,8 : {count}");
            Console.WriteLine($"Day 8 part 2: output value sum : {outputValueCount}");
        }

        private static void RunDay9()
        {
            var lines = File.ReadAllLines("InputData\\input9.txt").ToList();
            var heightGrid = lines;
            var riskLevels = new List<int>();
            var basinSizes = new List<int>();
            foreach (var heightLine in heightGrid)
            {
                var y = heightGrid.IndexOf(heightLine);
                for (int x = 0; x < heightLine.Length; x++)
                {
                    var height = heightLine[x];
                    var adjacentHeights = new List<int>();
                    if (x > 0)
                    {
                        adjacentHeights.Add(heightLine[x - 1]);
                    }

                    if (x < heightLine.Length - 1)
                    {
                        adjacentHeights.Add(heightLine[x + 1]);
                    }

                    if (y > 0)
                    {
                        adjacentHeights.Add(heightGrid[y - 1][x]);
                    }

                    if (y < heightGrid.Count - 1)
                    {
                        adjacentHeights.Add(heightGrid[y + 1][x]);
                    }

                    if (adjacentHeights.All(h => height < h))
                    {
                        riskLevels.Add(int.Parse($"{height}") + 1);
                        basinSizes.Add(FindBasinSize(x, y, heightGrid));
                    }
                }
            }

            var top3Basins = basinSizes.OrderByDescending(x => x).Take(3).ToList();

            Console.WriteLine($"Day 9 part 1: risk level sum: {riskLevels.Sum()}");
            Console.WriteLine($"Day 9 part 2: basin size product: {top3Basins[0] * top3Basins[1] * top3Basins[2]}");
        }
    }


}