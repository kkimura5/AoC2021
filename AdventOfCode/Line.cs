using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Line
    {

        public Line(Point startPoint, Point endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        public Point StartPoint { get; private set; }
        public Point EndPoint { get; private set; }

        public bool IsHorizontal => StartPoint.Y == EndPoint.Y;
        public bool IsVertical => StartPoint.X == EndPoint.X;

        public List<Point> GetPoints()
        {
            if (IsHorizontal)
            {
                var points = Enumerable.Range(Math.Min(StartPoint.X, EndPoint.X), Math.Abs(EndPoint.X - StartPoint.X) + 1).Select(x => new Point(x, StartPoint.Y)).ToList();
                return points;
            }
            else if (IsVertical)
            {
                var points = Enumerable.Range(Math.Min(StartPoint.Y, EndPoint.Y), Math.Abs(EndPoint.Y - StartPoint.Y) + 1).Select(y => new Point(StartPoint.X, y)).ToList();
                return points;
            }
            else
            {
                var deltaX = EndPoint.X - StartPoint.X;
                var deltaY = EndPoint.Y - StartPoint.Y;

                var points = new List<Point>();
                for (var point = StartPoint; point.X != EndPoint.X && point.Y != EndPoint.Y;)
                {
                    points.Add(point);
                    point.X += Math.Sign(deltaX);
                    point.Y += Math.Sign(deltaY);
                }

                points.Add(EndPoint);

                return points;
            }
        }

        public static Line Create(string textLine)
        {
            var pattern = @"(?<x1>\d+),(?<y1>\d+) -> (?<x2>\d+),(?<y2>\d+)";
            var match = Regex.Match(textLine, pattern);
            var startPoint = new Point(int.Parse(match.Groups["x1"].Value), int.Parse(match.Groups["y1"].Value));
            var endPoint = new Point(int.Parse(match.Groups["x2"].Value), int.Parse(match.Groups["y2"].Value));

            return new Line(startPoint, endPoint);
        }
    }
}
