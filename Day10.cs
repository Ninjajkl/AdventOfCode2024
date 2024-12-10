namespace AdventOfCode2024
{
    internal class Day10
    {
        private readonly List<(int r, int c)> adjacentPos =
            [
                (1, 0),
                (-1, 0),
                (0, 1),
                (0, -1),
            ];

        public string Part1(string inputName)
        {
            string[] RawInput = System.IO.File.ReadAllLines(inputName);

            Dictionary<(int r, int c), HashSet<(int r, int c)>> trails = [];
            PriorityQueue<(int r, int c, int value), int> frontier = new(Comparer<int>.Create((x, y) => y.CompareTo(x)));
            List<(int r, int c)> trailHeads = [];
            for (int r = 0; r < RawInput.Length; r++)
            {
                for (int c = 0; c < RawInput[r].Length; c++)
                {
                    if (RawInput[r][c] == '9')
                    {
                        frontier.Enqueue((r, c, 9), 9);
                        trails.Add((r, c), [(r, c)]);
                    }
                    else if (RawInput[r][c] == '0')
                    {
                        trailHeads.Add((r, c));
                    }
                }
            }

            while (frontier.Count > 0)
            {
                (int r, int c, int value) currTile = frontier.Dequeue();
                HashSet<(int r, int c)> currSet = trails[(currTile.r, currTile.c)];

                foreach ((int r, int c) adjust in adjacentPos)
                {
                    (int r, int c) = (currTile.r + adjust.r, currTile.c + adjust.c);

                    if (r < 0 || r >= RawInput.Length || c < 0 || c >= RawInput.Length) { continue; }
                    int value = RawInput[r][c] - '0';
                    if (value != currTile.value - 1) { continue; }

                    if (!trails.ContainsKey((r, c)))
                    {
                        trails.Add((r, c), []);
                        frontier.Enqueue((r, c, currTile.value - 1), currTile.value - 1);
                    }
                    trails[(r, c)] = trails[(r, c)].Union(currSet).ToHashSet();
                }
            }

            int totalScore = 0;
            foreach ((int r, int c) trailHead in trailHeads)
            {
                if (trails.TryGetValue(trailHead, out HashSet<(int r, int c)>? value))
                {
                    totalScore += value.Count;
                }
            }

            return totalScore.ToString();
        }

        public string Part2(string inputName)
        {
            string[] RawInput = System.IO.File.ReadAllLines(inputName);

            Dictionary<(int r, int c), List<(int r, int c)>> trails = [];
            PriorityQueue<(int r, int c, int value), int> frontier = new(Comparer<int>.Create((x, y) => y.CompareTo(x)));
            List<(int r, int c)> trailHeads = [];
            for (int r = 0; r < RawInput.Length; r++)
            {
                for (int c = 0; c < RawInput[r].Length; c++)
                {
                    if (RawInput[r][c] == '9')
                    {
                        frontier.Enqueue((r, c, 9), 9);
                        trails.Add((r, c), [(r, c)]);
                    }
                    else if (RawInput[r][c] == '0')
                    {
                        trailHeads.Add((r, c));
                    }
                }
            }

            while (frontier.Count > 0)
            {
                (int r, int c, int value) currTile = frontier.Dequeue();
                List<(int r, int c)> currSet = trails[(currTile.r, currTile.c)];

                foreach ((int r, int c) adjust in adjacentPos)
                {
                    (int r, int c) = (currTile.r + adjust.r, currTile.c + adjust.c);

                    if (r < 0 || r >= RawInput.Length || c < 0 || c >= RawInput.Length) { continue; }
                    int value = RawInput[r][c] - '0';
                    if (value != currTile.value - 1) { continue; }

                    if (!trails.ContainsKey((r, c)))
                    {
                        trails.Add((r, c), []);
                        frontier.Enqueue((r, c, currTile.value - 1), currTile.value - 1);
                    }
                    trails[(r, c)].AddRange(currSet);
                }
            }

            int totalScore = 0;
            foreach ((int r, int c) trailHead in trailHeads)
            {
                if (trails.TryGetValue(trailHead, out List<(int r, int c)>? value))
                {
                    totalScore += value.Count;
                }
            }

            return totalScore.ToString();
        }
    }
}