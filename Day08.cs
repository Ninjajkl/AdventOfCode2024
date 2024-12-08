namespace AdventOfCode2024
{
    internal class Day08
    {
        public string Part1(string inputName)
        {
            string[] RawInput = System.IO.File.ReadAllLines(inputName);

            HashSet<(int r, int c)> antinodeLocations = [];

            Dictionary<char, List<(int, int)>> antennaLocations = [];

            for (int r = 0; r < RawInput.Length; r++)
            {
                for (int c = 0; c < RawInput[r].Length; c++)
                {
                    if (RawInput[r][c] != '.')
                    {
                        if (!antennaLocations.ContainsKey(RawInput[r][c]))
                        {
                            antennaLocations[RawInput[r][c]] = new();
                        }
                        antennaLocations[RawInput[r][c]].Add((r, c));
                    }
                }
            }

            foreach (List<(int r, int c)> antennas in antennaLocations.Values)
            {
                List<(int r, int c)> testedAntennas = [antennas[0]];
                antennas.RemoveAt(0);

                foreach ((int r, int c) antenna in antennas)
                {
                    foreach ((int r, int c) prevAntenna in testedAntennas)
                    {
                        int rDiff = prevAntenna.r - antenna.r;
                        int cDiff = prevAntenna.c - antenna.c;
                        antinodeLocations.Add((prevAntenna.r + rDiff, prevAntenna.c + cDiff));
                        antinodeLocations.Add((antenna.r - rDiff, antenna.c - cDiff));
                    }
                    testedAntennas.Add(antenna);
                }
            }

            return antinodeLocations.Where(x => x.r >= 0 && x.r < RawInput.Length && x.c >= 0 && x.c < RawInput[0].Length).Count().ToString();
        }

        public string Part2(string inputName)
        {
            string[] RawInput = System.IO.File.ReadAllLines(inputName);

            HashSet<(int r, int c)> antinodeLocations = [];

            Dictionary<char, List<(int, int)>> antennaLocations = [];

            for (int r = 0; r < RawInput.Length; r++)
            {
                for (int c = 0; c < RawInput[r].Length; c++)
                {
                    if (RawInput[r][c] != '.')
                    {
                        if (!antennaLocations.ContainsKey(RawInput[r][c]))
                        {
                            antennaLocations[RawInput[r][c]] = new();
                        }
                        antennaLocations[RawInput[r][c]].Add((r, c));
                    }
                }
            }

            foreach (List<(int r, int c)> antennas in antennaLocations.Values)
            {
                List<(int r, int c)> testedAntennas = [antennas[0]];
                antennas.RemoveAt(0);

                foreach ((int r, int c) antenna in antennas)
                {
                    foreach ((int r, int c) prevAntenna in testedAntennas)
                    {
                        int rDiff = prevAntenna.r - antenna.r;
                        int cDiff = prevAntenna.c - antenna.c;

                        (int r, int c) nextPos = prevAntenna;
                        while (nextPos.r >= 0 && nextPos.r < RawInput.Length && nextPos.c >= 0 && nextPos.c < RawInput[0].Length)
                        {
                            antinodeLocations.Add(nextPos);
                            nextPos = (nextPos.r + rDiff, nextPos.c + cDiff);
                        }
                        nextPos = prevAntenna;
                        while (nextPos.r >= 0 && nextPos.r < RawInput.Length && nextPos.c >= 0 && nextPos.c < RawInput[0].Length)
                        {
                            antinodeLocations.Add(nextPos);
                            nextPos = (nextPos.r - rDiff, nextPos.c - cDiff);
                        }
                    }
                    testedAntennas.Add(antenna);
                }
            }

            return antinodeLocations.Count().ToString();
        }
    }
}