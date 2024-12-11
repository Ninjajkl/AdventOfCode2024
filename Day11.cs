namespace AdventOfCode2024
{
    internal class Day11
    {
        public string Part1(string inputName)
        {
            return GetStoneCount(inputName, 25);
        }

        public string Part2(string inputName)
        {
            return GetStoneCount(inputName, 75);
        }

        public static string GetStoneCount(string inputName, int count)
        {
            string RawInput = File.ReadAllLines(inputName)[0];

            Dictionary<long, long> stones = RawInput
                .Split(" ")
                .Select(long.Parse)
                .GroupBy(x => x)
                .ToDictionary(g => g.Key, g => (long)g.Count());

            stones[1] = 0;
            for (int i = 0; i < count; i++)
            {
                foreach (KeyValuePair<long, long> kvp in new Dictionary<long, long>(stones))
                {
                    long stoneValue = kvp.Key;
                    long stoneCount = kvp.Value;
                    if (stoneCount == 0)
                    {
                        continue;
                    }
                    if (stoneValue == 0)
                    {

                        stones[1] += stoneCount;
                        stones[stoneValue] -= stoneCount;
                        continue;
                    }
                    string stoneString = stoneValue.ToString();
                    if (stoneString.Length % 2 == 0)
                    {
                        long firstStone = long.Parse(stoneString[..(stoneString.Length/2)]);
                        long secondStone = long.Parse(stoneString[(stoneString.Length/2)..]);
                        if (!stones.ContainsKey(firstStone))
                        {
                            stones[firstStone] = 0;
                        }
                        stones[firstStone] += stoneCount;
                        if (!stones.ContainsKey(secondStone))
                        {
                            stones[secondStone] = 0;
                        }
                        stones[secondStone] += stoneCount;
                        stones[stoneValue] -= stoneCount;
                    }
                    else
                    {
                        long newValue = stoneValue*2024;
                        if (!stones.ContainsKey(newValue))
                        {
                            stones[newValue] = 0;
                        }
                        stones[newValue] += stoneCount;
                        stones[stoneValue] -= stoneCount;
                    }

                }
            }
            return stones.Values.Sum().ToString();
        }
    }
}