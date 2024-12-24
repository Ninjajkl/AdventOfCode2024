using System.Text;
using static AdventOfCode2024.Day24.Wiring;

namespace AdventOfCode2024
{
    internal class Day24
    {
        public class Wiring
        {
            public string Name { get; set; }
            public string Val1 { get; set; }
            public string Val2 { get; set; }
            public op Op { get; set; }
            Dictionary<string, Wiring> WireValues { get; set; }
            public HashSet<Wiring> Dependencies { get; set; }

            public bool StartingWire = false;
            bool val = false;
            public bool Val
            {
                get
                {
                    if (!StartingWire)
                    {
                        val = Op switch
                        {
                            op.AND => WireValues[Val1].Val & WireValues[Val2].Val,
                            op.OR => WireValues[Val1].Val | WireValues[Val2].Val,
                            op.XOR => WireValues[Val1].Val ^ WireValues[Val2].Val
                        };
                        Dependencies = WireValues[Val1].Dependencies.Union(WireValues[Val2].Dependencies).ToHashSet();
                        Dependencies.Add(WireValues[Val1]);
                        Dependencies.Add(WireValues[Val2]);
                    }
                    return val;
                }
            }

            public Wiring(string name, bool value)
            {
                Dependencies = [];
                Name = name;
                StartingWire = true;
                val = value;
            }

            public Wiring(string name, string val1, string val2, op op, Dictionary<string, Wiring> wireValues)
            {
                Dependencies = [];
                Name = name;
                Val1 = val1;
                Val2 = val2;
                Op = op;
                WireValues = wireValues;
            }

            public enum op
            {
                AND,
                OR,
                XOR
            }

        }

        public string Part1(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            Dictionary<string, Wiring> wireValues = [];

            List<Wiring> zWires = [];
            bool operations = false;
            foreach (var line in RawInput)
            {
                if (operations)
                {
                    string[] parts = line.Split(' ');
                    string var1 = parts[0];
                    string var2 = parts[2];
                    op operation = (op)Enum.Parse(typeof(op), parts[1]);
                    string wireName = parts.Last();
                    Wiring newWire = new(wireName, var1, var2, operation, wireValues);
                    wireValues.Add(wireName, newWire);
                    if (wireName[0] == 'z')
                    {
                        zWires.Add(newWire);
                    }
                }
                else if (line.Length == 0)
                {
                    operations = true;
                }
                else
                {
                    string wireName = line[..3];
                    wireValues.Add(wireName, new(wireName, line.Last() == '1'));
                }
            }
            zWires = [.. zWires.OrderByDescending(x => x.Name)];

            StringBuilder wireGrapher = new();
            foreach (var wire in zWires)
            {
                wireGrapher.Append(wire.Val ? "1" : "0");
            }
            return Convert.ToInt64(wireGrapher.ToString(), 2).ToString();
        }

        public string Part2(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            Dictionary<string, Wiring> wireValues = [];

            List<Wiring> zWires = [];
            bool operations = false;
            foreach (var line in RawInput)
            {
                if (operations)
                {
                    string[] parts = line.Split(' ');
                    string var1 = parts[0];
                    string var2 = parts[2];
                    op operation = (op)Enum.Parse(typeof(op), parts[1]);
                    string wireName = parts.Last();
                    Wiring newWire = new(wireName, var1, var2, operation, wireValues);
                    wireValues.Add(wireName, newWire);
                    if (wireName[0] == 'z')
                    {
                        zWires.Add(newWire);
                    }
                }
                else if (line.Length == 0)
                {
                    operations = true;
                }
                else
                {
                    string wireName = line[..3];
                    wireValues.Add(wireName, new(wireName, line.Last() == '1'));
                }
            }
            zWires = [.. zWires.OrderByDescending(x => x.Name)];

            Wiring lastZGate = zWires.First();
            var faultyGates = new HashSet<Wiring>();

            foreach (Wiring wiring in wireValues.Values)
            {
                bool isFaulty = false;

                if (wiring.StartingWire)
                {
                    continue;
                }
                if (wiring.Name.StartsWith('z') && wiring.Name != lastZGate.Name)
                {
                    isFaulty = wiring.Op != op.XOR;
                }
                else if (!wiring.Name.StartsWith('z') && !wireValues[wiring.Val1].StartingWire && !wireValues[wiring.Val2].StartingWire)
                {
                    isFaulty = wiring.Op == op.XOR;
                }
                else if (wireValues[wiring.Val1].StartingWire && wireValues[wiring.Val2].StartingWire && !wiring.Val1.EndsWith("00") && !wiring.Val2.EndsWith("00"))
                {
                    string output = wiring.Name;
                    var expectedNextType = wiring.Op == op.XOR ? op.XOR : op.OR;

                    bool feedsIntoExpectedGate = wireValues.Values.Any(other =>
                        other != wiring &&
                        (other.Val1 == output || other.Val2 == output) &&
                        other.Op == expectedNextType);

                    isFaulty = !feedsIntoExpectedGate;
                }

                if (isFaulty)
                {
                    faultyGates.Add(wiring);
                }
            }

            return string.Join(",", faultyGates.Select(g => g.Name).OrderBy(w => w));
        }
    }
}