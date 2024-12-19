namespace AdventOfCode2024
{
    internal class Day18
    {
        private readonly List<(int r, int c)> adjacentPos =
            [
                (1, 0),
                (-1, 0),
                (0, 1),
                (0, -1),
            ];

        public class Node
        {
            public int R { get; set; }
            public int C { get; set; }
            public Node Prev { get; set; }

            public Node(int r, int c, Node prev)
            {
                R = r;
                C = c;
                Prev = prev;
            }
        }

        public string Part1(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            char[,] memorySpace = RawInput.Length < 30 ? new char[7, 7] : new char[71, 71];

            for (int i = 0; i < (RawInput.Length < 30 ? 12 : 1024); i++)
            {
                string[] parts = RawInput[i].Split(",");
                memorySpace[int.Parse(parts[0]), int.Parse(parts[1])] = '#';
            }

            Node start = new(0, 0, null);
            (int r, int c) end = (memorySpace.GetLength(0) - 1, memorySpace.GetLength(1) - 1);

            Queue<Node> frontier = new();
            frontier.Enqueue(start);

            HashSet<(int r, int c)> visited = new();

            Node endNode = new(-1, -1, null);
            while (frontier.Count > 0)
            {
                Node curr = frontier.Dequeue();
                if (visited.Contains((curr.R, curr.C)))
                {
                    continue;
                }
                visited.Add((curr.R, curr.C));

                if ((curr.R, curr.C) == end)
                {
                    endNode = curr;
                    break;
                }
                foreach ((int rr, int cc) in adjacentPos)
                {
                    int newR = curr.R + rr;
                    int newC = curr.C + cc;
                    if (newR < 0 || newR >= memorySpace.GetLength(0) || newC < 0 || newC >= memorySpace.GetLength(1) || memorySpace[newR, newC] == '#')
                    {
                        continue;
                    }
                    frontier.Enqueue(new(newR, newC, curr));
                }
            }

            int steps = -1;
            Node unwindNode = endNode;
            while (unwindNode != null)
            {
                steps++;
                unwindNode = unwindNode.Prev;
            }
            return steps.ToString();
        }

        public string Part2(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            char[,] memorySpace = RawInput.Length < 30 ? new char[7, 7] : new char[71, 71];

            Queue<(int r, int c)> byteBlockers = [];
            for (int l = 0; l < RawInput.Length; l++)
            {
                string[] parts = RawInput[l].Split(",");
                byteBlockers.Enqueue((int.Parse(parts[0]), int.Parse(parts[1])));
            }

            int i;
            for (i = 0; i < (RawInput.Length < 30 ? 12 : 1024); i++)
            {
                (int r, int c) = byteBlockers.Dequeue();
                memorySpace[r, c] = '#';
            }

            (int r, int c) nextBlocker = (-1, -1);
            while (true)
            {
                Node start = new(0, 0, null);
                (int r, int c) end = (memorySpace.GetLength(0) - 1, memorySpace.GetLength(1) - 1);

                Queue<Node> frontier = new();
                frontier.Enqueue(start);

                HashSet<(int r, int c)> visited = new();

                Node endNode = new(-1, -1, null);
                while (frontier.Count > 0)
                {
                    Node curr = frontier.Dequeue();
                    if (visited.Contains((curr.R, curr.C)))
                    {
                        continue;
                    }
                    visited.Add((curr.R, curr.C));

                    if ((curr.R, curr.C) == end)
                    {
                        endNode = curr;
                        break;
                    }
                    foreach ((int rr, int cc) in adjacentPos)
                    {
                        int newR = curr.R + rr;
                        int newC = curr.C + cc;
                        if (newR < 0 || newR >= memorySpace.GetLength(0) || newC < 0 || newC >= memorySpace.GetLength(1) || memorySpace[newR, newC] == '#')
                        {
                            continue;
                        }
                        frontier.Enqueue(new(newR, newC, curr));
                    }
                }
                if (endNode.R == -1)
                {
                    break;
                }
                nextBlocker = byteBlockers.Dequeue();
                memorySpace[nextBlocker.r, nextBlocker.c] = '#';
            }

            return $"{nextBlocker.r},{nextBlocker.c}";
        }
    }
}