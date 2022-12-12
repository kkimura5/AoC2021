using System;
using System.Linq;

namespace AdventOfCode
{
    public class PacketTracker
    {
        public string PacketText { get; private set; }
        public long Marker { get;set; }
        public PacketTracker(string packetText)
        {
            this.PacketText = packetText;
        }

        public long GetNextBitsValue(int numBits)
        {
            var numCharsToSkip = Marker / 4;
            var numCharsToGrab = numBits / 4 + 2;
            while (numCharsToSkip + numCharsToGrab > PacketText.Length)
            {
                numCharsToGrab--;
            }

            var text = PacketText.Substring((int)numCharsToSkip, numCharsToGrab);
            var value = Convert.ToUInt32(text, 16);

            var numBitsToSkip = Marker % 4 + 4 * (8 - numCharsToGrab);
            var numBitsAtEnd = (int)(32 - (numBitsToSkip + numBits));
            
            var mask = Convert.ToUInt32(string.Join("", Enumerable.Range(0, numBits).Select(x => "1")), 2);

            Marker += numBits;

            return (value >> numBitsAtEnd) & mask;
        }
    }
}