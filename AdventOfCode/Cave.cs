using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Cave
    {
        public Cave(string name)
        {
            Name = name;
        }
        public string Name { get; private set; }
        public bool IsLargeCave => Name == Name.ToUpper();
        public List<Cave> LinkedCaves { get; private set; } = new List<Cave>();
        public bool IsStart => Name == "start";
        public bool IsEnd => Name == "end";

        internal void LinkToCave(Cave cave2)
        {
            LinkedCaves.Add(cave2);
            cave2.LinkedCaves.Add(this);
        }
        public override string ToString()
        {
            return Name;
        }
        public static List<Cave> CreateFromText(List<string> lines)
        {
            var caves = new List<Cave>();
            var pattern = @"(?<start>\w+)-(?<end>\w+)";
            foreach (var line in lines)
            {
                var match = Regex.Match(line, pattern);
                var cave1Name = match.Groups["start"].Value;
                var cave2Name = match.Groups["end"].Value;
                var cave1 = caves.SingleOrDefault(x => x.Name == cave1Name);
                var cave2 = caves.SingleOrDefault(x => x.Name == cave2Name);

                if (cave1 is null)
                {
                    cave1 = new Cave(cave1Name);
                    caves.Add(cave1);
                }

                if (cave2 is null)
                {
                    cave2 = new Cave(cave2Name);
                    caves.Add(cave2);
                }

                cave1.LinkToCave(cave2);
            }

            return caves;
        }
    }
}
