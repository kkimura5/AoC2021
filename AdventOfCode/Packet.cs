using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Packet
    {
        public int Version { get; set; }
        public int Id { get; set; }
        public long LiteralValue { get; set; }
        public List<Packet> SubPackets { get; set; } = new List<Packet>();
        public int NumBits { get; set; }
        public static Packet FromText(PacketTracker packetTracker)
        {
            var packet = new Packet();            
            packet.Version = (int)packetTracker.GetNextBitsValue(3);
            packet.Id = (int)packetTracker.GetNextBitsValue(3);
            packet.NumBits += 6;

            if (packet.Id == 4)
            {
                var lastDigitFound =false;
                long literalValue = 0;
                var totalBits = 0;
                while (!lastDigitFound)
                {
                    var digit = packetTracker.GetNextBitsValue(5);
                    totalBits += 5;
                    lastDigitFound = (digit & 0b10000) == 0;
                    literalValue = (literalValue << 4) + (digit & 0b1111);
                }                

                packet.NumBits += totalBits;
                packet.LiteralValue = literalValue;
            }
            else
            {
                var lengthTypeId = packetTracker.GetNextBitsValue(1);
                packet.NumBits++;
                
                if (lengthTypeId == 0)
                {
                    var numBits = packetTracker.GetNextBitsValue(15);
                    packet.NumBits += 15;
                    var subPacketBitCount = 0;
                    while (subPacketBitCount < numBits)
                    {
                        var subPacket = FromText(packetTracker);
                        packet.SubPackets.Add(subPacket);
                        packet.NumBits += subPacket.NumBits;
                        subPacketBitCount += subPacket.NumBits;
                    }
                }
                else
                {
                    var numPackets = packetTracker.GetNextBitsValue(11); 
                    packet.NumBits += 11;
                    for (int i = 0; i < numPackets; i++)
                    {
                        var subPacket = FromText(packetTracker);
                        packet.SubPackets.Add(subPacket);
                        packet.NumBits += subPacket.NumBits;
                    }
                }
            }

            return packet;
        }

        public List<int> GetVersions()
        {
            var versions = new List<int>();
            versions.Add(Version);
            versions.AddRange(SubPackets.SelectMany(x => x.GetVersions()));

            return versions;
        }

        public long GetPacketValue()
        {
            switch (Id)
            {
                case 0:
                    return SubPackets.Sum(x => x.GetPacketValue());

                case 1:
                    var value = 1L;
                    foreach ( var subPacket in SubPackets)
                    {
                        value *= subPacket.GetPacketValue();
                    }

                    return value;

                case 2:
                    return SubPackets.Min(x => x.GetPacketValue());

                case 3:
                    return SubPackets.Max(x => x.GetPacketValue());

                case 4:
                    return LiteralValue;

                case 5:
                    return SubPackets.First().GetPacketValue() > SubPackets.Last().GetPacketValue() ? 1 : 0;

                case 6:
                    return SubPackets.First().GetPacketValue() < SubPackets.Last().GetPacketValue() ? 1 : 0;

                case 7:
                    return SubPackets.First().GetPacketValue() == SubPackets.Last().GetPacketValue() ? 1 : 0;
            }

            throw new Exception();
        }
    }
}
