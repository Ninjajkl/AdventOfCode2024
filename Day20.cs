namespace AdventOfCode2024
{
    internal class Day20
    {
        private readonly List<(int r, int c)> adjPos =
            [
                (1, 0),
                (-1, 0),
                (0, 1),
                (0, -1),
            ];

        public string Part1(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            (int r, int c) startNode = (-1, -1);
            (int r, int c) endNode = (-1, -1);

            for (int r = 0; r < RawInput.Length; r++)
            {
                for (int c = 0; c < RawInput[r].Length; c++)
                {
                    if (RawInput[r][c] == 'S')
                    {
                        startNode = (r, c);
                    }
                    else if (RawInput[r][c] == 'E')
                    {
                        endNode = (r, c);
                    }
                }
            }

            Dictionary<int, int> cheats = [];
            Dictionary<(int r, int c), int> prevNodes = [];

            (int r, int c) currNode = startNode;
            int t = 0;
            while (true)
            {
                (int r, int c) = currNode;
                prevNodes.Add(currNode, t);
                for (int rr = -1; rr <= 1; rr++)
                {
                    for (int cc = -1; cc <= 1; cc++)
                    {
                        if (Math.Abs(rr) + Math.Abs(cc) != 1)
                        {
                            continue;
                        }
                        (int pr, int pc) = (r + (rr * 2), c + (cc * 2));
                        if (!prevNodes.TryGetValue((pr, pc), out int pt))
                        {
                            continue;
                        }
                        int timeSkip = t - pt - 2;
                        if (timeSkip == 0)
                        {
                            continue;
                        }

                        if (!cheats.ContainsKey(timeSkip))
                        {
                            cheats.Add(timeSkip, 1);
                        }
                        else
                        {
                            cheats[timeSkip] += 1;
                        }
                    }
                }

                if (currNode == endNode)
                {
                    break;
                }

                foreach ((int rr, int cc) in adjPos)
                {
                    (int pr, int pc) = (r + (rr * 1), c + (cc * 1));
                    if (pr < 0 || pr >= RawInput.Length || pc < 0 || pc >= RawInput[0].Length || RawInput[pr][pc] == '#')
                    {
                        continue;
                    }
                    if (prevNodes.ContainsKey((pr, pc)))
                    {
                        continue;
                    }
                    currNode = (pr, pc);
                    break;
                }
                t++;
            }

            return cheats.Where(kv => kv.Key >= 100).Sum(kv => kv.Value).ToString();
        }

        public string Part2(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            (int r, int c) startNode = (-1, -1);
            (int r, int c) endNode = (-1, -1);

            for (int r = 0; r < RawInput.Length; r++)
            {
                for (int c = 0; c < RawInput[r].Length; c++)
                {
                    if (RawInput[r][c] == 'S')
                    {
                        startNode = (r, c);
                    }
                    else if (RawInput[r][c] == 'E')
                    {
                        endNode = (r, c);
                    }
                }
            }

            Dictionary<int, int> cheats = [];
            Dictionary<(int r, int c), int> prevNodes = [];

            (int r, int c) currNode = startNode;
            int t = 0;
            while (true)
            {
                (int r, int c) = currNode;
                prevNodes.Add(currNode, t);
                if (t > 50)
                {
                    for (int rr = -20; rr <= 20; rr++)
                    {
                        for (int cc = -20; cc <= 20; cc++)
                        {
                            int distance = Math.Abs(rr) + Math.Abs(cc);
                            if (distance > 20)
                            {
                                continue;
                            }
                            (int pr, int pc) = (r + rr, c + cc);
                            if (!prevNodes.TryGetValue((pr, pc), out int pt))
                            {
                                continue;
                            }
                            int timeSkip = t - pt - distance;
                            if (timeSkip is 0 or < 50)
                            {
                                continue;
                            }

                            if (!cheats.ContainsKey(timeSkip))
                            {
                                cheats.Add(timeSkip, 1);
                            }
                            else
                            {
                                cheats[timeSkip] += 1;
                            }
                        }
                    }
                }

                if (currNode == endNode)
                {
                    break;
                }

                foreach ((int rr, int cc) in adjPos)
                {
                    (int pr, int pc) = (r + (rr * 1), c + (cc * 1));
                    if (pr < 0 || pr >= RawInput.Length || pc < 0 || pc >= RawInput[0].Length || RawInput[pr][pc] == '#')
                    {
                        continue;
                    }
                    if (prevNodes.ContainsKey((pr, pc)))
                    {
                        continue;
                    }
                    currNode = (pr, pc);
                    break;
                }
                t++;
            }

            return cheats.Where(kv => kv.Key >= 100).Sum(kv => kv.Value).ToString();
        }
    }
}