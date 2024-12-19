namespace AdventOfCode2024
{
    internal class Day19
    {
        public string Part1(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            string[] patterns = RawInput[0].Split(", ");
            string[] onsens = RawInput.Skip(2).ToArray();
            int possible = 0;
            foreach (string onsen in onsens)
            {
                Dictionary<int, List<int>> patternPositions = patterns
                    .SelectMany(pattern => Enumerable.Range(0, onsen.Length - pattern.Length + 1)
                                                     .Where(i => onsen.IndexOf(pattern, i) == i)
                                                     .Select(i => new { Index = i, pattern.Length }))
                    .GroupBy(x => x.Index)
                    .ToDictionary(g => g.Key, g => g.Select(x => x.Length)
                                                    .OrderByDescending(length => length)
                                                    .ToList());
                possible += FindPattern(patternPositions, 0, onsen, []) ? 1 : 0;
            }
            return possible.ToString();
        }

        public static bool FindPattern(Dictionary<int, List<int>> patternPositions, int currPos, string onsen, HashSet<int> explored)
        {
            if (explored.Contains(currPos))
            {
                return false;
            }
            explored.Add(currPos);

            if (currPos == onsen.Length)
            {
                return true;
            }

            if (!patternPositions.TryGetValue(currPos, out List<int> patternsForIndex))
            {
                return false;
            }

            foreach (int patternLength in patternsForIndex)
            {
                if (FindPattern(patternPositions, currPos + patternLength, onsen, explored))
                {
                    return true;
                }
            }
            return false;
        }

        public string Part2(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            string[] patterns = RawInput[0].Split(", ");
            string[] onsens = RawInput.Skip(2).ToArray();
            long possible = 0;
            foreach (string onsen in onsens)
            {
                Dictionary<int, List<int>> patternPositions = patterns
                    .SelectMany(pattern => Enumerable.Range(0, onsen.Length - pattern.Length + 1)
                                                     .Where(i => onsen.IndexOf(pattern, i) == i)
                                                     .Select(i => new { Index = i, pattern.Length }))
                    .GroupBy(x => x.Index)
                    .ToDictionary(g => g.Key, g => g.Select(x => x.Length)
                                                    .OrderByDescending(length => length)
                                                    .ToList());
                Dictionary<int, long> explored = [];

                PriorityQueue<(int c, int p), int> frontier = new();
                frontier.Enqueue((0, -1), 0);

                while (frontier.Count > 0)
                {
                    (int currIndex, int prev) = frontier.Dequeue();
                    if (explored.ContainsKey(currIndex))
                    {
                        explored[currIndex] += explored[prev];
                        continue;
                    }
                    explored[currIndex] = prev == -1 ? 1 : explored[prev];

                    if (!patternPositions.ContainsKey(currIndex))
                    {
                        continue;
                    }

                    foreach (int patternLength in patternPositions[currIndex])
                    {
                        frontier.Enqueue((currIndex + patternLength, currIndex), currIndex + patternLength);
                    }
                }

                long possibleHere = 0;

                if (explored.TryGetValue(onsen.Length, out long successful))
                {
                    possibleHere = successful;
                }
                possible += possibleHere;
            }
            return possible.ToString();
        }
    }
}