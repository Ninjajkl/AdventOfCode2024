using System.Text;

namespace AdventOfCode2024
{
    internal class Day21
    {
        readonly Dictionary<char, (int r, int c)> buttonMap = new()
            {
                { '^', (0, 1) },
                { 'A', (0, 2) },
                { '<', (1, 0) },
                { 'v', (1, 1) },
                { '>', (1, 2) }
            };

        public string Part1(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            Dictionary<(string code, int depth), long> codeMap = new();

            long total = 0;
            foreach (string Code in RawInput)
            {
                List<string> startingSequence = MapNumeric(Code);

                long complexity = long.Parse(Code[..^1]);
                long instructionsLength = 0;
                foreach (string sequence in startingSequence)
                {
                    instructionsLength += GetShortestLength(sequence, codeMap, 1, 2);
                }
                complexity *= instructionsLength;
                total += complexity;
            }
            return total.ToString();
        }

        public string Part2(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            Dictionary<(string code, int depth), long> codeMap = new();

            long total = 0;
            foreach (string Code in RawInput)
            {
                List<string> startingSequence = MapNumeric(Code);

                long complexity = long.Parse(Code[..^1]);
                long instructionsLength = 0;
                foreach (string sequence in startingSequence)
                {
                    instructionsLength += GetShortestLength(sequence, codeMap, 1, 25);
                }
                complexity *= instructionsLength;
                total += complexity;
            }
            return total.ToString();
        }

        public static List<string> MapNumeric(string neededButtons)
        {
            Dictionary<char, (int r, int c)> p1ButtonMap = new()
            {
                { '7', (0, 0) },
                { '8', (0, 1) },
                { '9', (0, 2) },
                { '4', (1, 0) },
                { '5', (1, 1) },
                { '6', (1, 2) },
                { '1', (2, 0) },
                { '2', (2, 1) },
                { '3', (2, 2) },
                { '0', (3, 1) },
                { 'A', (3, 2) }
            };

            string checkButtons = "A" + neededButtons;

            List<string> sequenceParts = [];
            for (int i = 1; i < checkButtons.Length; i++)
            {
                var prevButton = checkButtons[i-1];
                var currButton = checkButtons[i];

                int rDiff = p1ButtonMap[prevButton].r - p1ButtonMap[currButton].r;
                int cDiff = p1ButtonMap[prevButton].c - p1ButtonMap[currButton].c;

                int down = rDiff < 0 ? Math.Abs(rDiff) : 0;
                int up = rDiff > 0 ? rDiff : 0;

                int right = cDiff < 0 ? Math.Abs(cDiff) : 0;
                int left = cDiff > 0 ? cDiff : 0;

                if (p1ButtonMap[prevButton].r == 3 && p1ButtonMap[currButton].c == 0)
                {
                    sequenceParts.Add(new StringBuilder().Append('^', up).Append('<', left).Append('A').ToString());
                    continue;
                }
                else if (p1ButtonMap[currButton].r == 3 && p1ButtonMap[prevButton].c == 0)
                {
                    sequenceParts.Add(new StringBuilder().Append('>', right).Append('v', down).Append('A').ToString());
                    continue;
                }
                sequenceParts.Add(new StringBuilder().Append('<', left).Append('v', down).Append('^', up).Append('>', right).Append('A').ToString());
            }
            return sequenceParts;
        }

        public long GetShortestLength(string prevSequence, Dictionary<(string code, int depth), long> codeMap, int depth, int depthLimit)
        {
            long total = 0;
            if (codeMap.TryGetValue((prevSequence, depth), out long length))
            {
                total = length;
            }
            else if (depth <= depthLimit)
            {
                string checkButtons = "A" + prevSequence;

                StringBuilder sequence = new();
                for (int i = 1; i < checkButtons.Length; i++)
                {
                    var prevButton = checkButtons[i-1];
                    var currButton = checkButtons[i];

                    int rDiff = buttonMap[prevButton].r - buttonMap[currButton].r;
                    int cDiff = buttonMap[prevButton].c - buttonMap[currButton].c;

                    int down = rDiff < 0 ? Math.Abs(rDiff) : 0;
                    int up = rDiff > 0 ? rDiff : 0;

                    int right = cDiff < 0 ? Math.Abs(cDiff) : 0;
                    int left = cDiff > 0 ? cDiff : 0;

                    if (buttonMap[prevButton].r == 0 && buttonMap[currButton].c == 0)
                    {
                        sequence.Append('v', down).Append('<', left).Append('A');
                    }
                    else if (buttonMap[currButton].r == 0 && buttonMap[prevButton].c == 0)
                    {
                        sequence.Append('>', right).Append('^', up).Append('A');
                    }
                    else
                    {
                        sequence.Append('<', left).Append('v', down).Append('^', up).Append('>', right).Append('A');
                    }
                    total += GetShortestLength(sequence.ToString(), codeMap, depth + 1, depthLimit);
                    sequence = new();
                }
            }
            else
            {
                total = prevSequence.Length;
            }
            codeMap[(prevSequence, depth)] = total;
            //Console.WriteLine($"{prevSequence}, {depth}, {total}");
            return total;
        }
    }
}