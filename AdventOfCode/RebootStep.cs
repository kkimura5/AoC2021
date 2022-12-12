using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class RebootStep
    {
        public RebootStep(string text)
        {
            IsOn = text.Contains("on");
            var pattern = @"x=(?<xMin>-?\d+)\.\.(?<xMax>-?\d+),y=(?<yMin>-?\d+)\.\.(?<yMax>-?\d+),z=(?<zMin>-?\d+)\.\.(?<zMax>-?\d+)";

            var match = Regex.Match(text, pattern);
            XMin = int.Parse(match.Groups["xMin"].Value);
            XMax = int.Parse(match.Groups["xMax"].Value);
            YMin = int.Parse(match.Groups["yMin"].Value);
            YMax = int.Parse(match.Groups["yMax"].Value);
            ZMin = int.Parse(match.Groups["zMin"].Value);
            ZMax = int.Parse(match.Groups["zMax"].Value);
        }

        public RebootStep()
        {
        }

        public static RebootStep NullStep => new RebootStep() { IsNull = true };
        public int XMin { get; set; }
        public int XMax { get; set; }
        public int YMin { get; set; }
        public int YMax { get; set; }
        public int ZMin { get; set; }
        public int ZMax { get; set; }


        public bool IsOn { get; set; }

        public long NumCubes => (XMax - XMin + 1) * (YMax - YMin + 1) * (ZMax - ZMin + 1);

        public bool IsNull { get; private set; }

        public override string ToString()
        {
            return $"X {XMin}..{XMax},Y {YMin}..{YMax},Z {ZMin}..{ZMax}";
        }
        public RebootStep GetIntersection(RebootStep other)
        {
            int xMin, yMin, zMin, xMax, yMax, zMax;
            xMin = yMin = zMin = xMax = yMax = zMax = int.MinValue;
            if (XMin.IsBetween(other.XMin, other.XMax))
            {
                xMin = XMin;
                xMax = Math.Min(XMax, other.XMax);
            }
            else if (XMax.IsBetween(other.XMin, other.XMax))
            {
                xMin = Math.Max(XMin, other.XMin);
                xMax = XMax;
            }

            if (YMin.IsBetween(other.YMin, other.YMax))
            {
                yMin = YMin;
                yMax = Math.Min(YMax, other.YMax);
            }
            else if (YMax.IsBetween(other.YMin, other.YMax))
            {
                yMin = Math.Max(YMin, other.YMin);
                yMax = YMax;
            }

            if (ZMin.IsBetween(other.ZMin, other.ZMax))
            {
                zMin = ZMin;
                zMax = Math.Min(ZMax, other.ZMax);
            }
            else if (ZMax.IsBetween(other.ZMin, other.ZMax))
            {
                zMin = Math.Max(ZMin, other.ZMin);
                zMax = ZMax;
            }           

            if (new List<int> { xMin, xMax, yMin, yMax, zMin, zMax }.All(x => x == int.MinValue))
            {
                return NullStep;
            }

            return new RebootStep()
            {
                XMin = xMin,
                XMax = xMax,
                YMax = yMax,
                YMin = yMin,
                ZMin = zMin,
                ZMax = zMax,
                IsOn = !IsOn
            };
        }

        internal bool IsInRange(int range)
        {
            return Math.Abs(XMin) <= range && Math.Abs(XMax) <= range
                && Math.Abs(YMin) <= range && Math.Abs(YMax) <= range
                && Math.Abs(ZMin) <= range && Math.Abs(ZMax) <= range;
        }

        internal List<Point3D> GetPoints()
        {
            var points = new List<Point3D>();
            for (int x = XMin; x <= XMax; x++)
            {
                for (int y = YMin; y <= YMax; y++)
                {
                    for (int z = ZMin; z <=ZMax; z++)
                    {
                        points.Add(new Point3D(x, y, z));
                    }
                }
            }
            return points;
        }


    }
}
