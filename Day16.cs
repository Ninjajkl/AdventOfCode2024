namespace AdventOfCode2024
{
    internal class Day16
    {
        private class Node
        {
            public int Row { get; set; }
            public int Col { get; set; }
            public int G { get; set; }
            public int H => Math.Abs(Row - EndPoint.r) + Math.Abs(Col - EndPoint.c);
            public int F => G + H;
            public Direc Dir { get; set; }
            public HashSet<Node> PrevNodes { get; set; }
            public (int r, int c) EndPoint { get; set; }

            public Node(int r, int c, int g, Direc dir, Node pNode, (int r, int c) ePoint)
            {
                Row = r;
                Col = c;
                G = g;
                Dir = dir;
                PrevNodes = [pNode];
                EndPoint = ePoint;
            }

            public Node(int r, int c, (int r, int c) ePoint)
            {
                Row = r;
                Col = c;
                G = 0;
                Dir = Direc.right;
                PrevNodes = [];
                EndPoint = ePoint;
            }
        }
        private enum Direc
        {
            up,
            down,
            left,
            right
        }

        public string Part1(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            (int r, int c) startPos = (-1, -1);
            (int r, int c) endPos = (-1, -1);
            for (int r = 0; r < RawInput.Length; r++)
            {
                for (int c = 0; c < RawInput[r].Length; c++)
                {
                    if (RawInput[r][c] is 'S')
                    {
                        startPos = (r, c);
                    }
                    else if (RawInput[r][c] is 'E')
                    {
                        endPos = (r, c);
                    }
                }
            }

            Node start = new(startPos.r, startPos.c, endPos);

            PriorityQueue<Node, int> frontier = new();
            frontier.Enqueue(start, 0);
            HashSet<(int r, int c, Direc dir)> visited = [];

            Node endNode = null;
            while (frontier.Count > 0)
            {
                Node cNode = frontier.Dequeue();
                visited.Add((cNode.Row, cNode.Col, cNode.Dir));

                if (cNode.Row == endPos.r && cNode.Col == endPos.c)
                {
                    endNode = cNode;
                    break;
                }

                List<(int r, int c, int cost, Direc dir)> nextPos = cNode.Dir switch
                {
                    Direc.up =>
                        [
                            (cNode.Row - 1, cNode.Col, 1, Direc.up),
                            (cNode.Row, cNode.Col - 1, 1001, Direc.left),
                            (cNode.Row, cNode.Col + 1, 1001, Direc.right)
                        ],
                    Direc.down =>
                        [
                            (cNode.Row + 1, cNode.Col, 1, Direc.down),
                            (cNode.Row, cNode.Col - 1, 1001, Direc.left),
                            (cNode.Row, cNode.Col + 1, 1001, Direc.right)
                        ],
                    Direc.left =>
                        [
                            (cNode.Row - 1, cNode.Col, 1001, Direc.up),
                            (cNode.Row + 1, cNode.Col, 1001, Direc.down),
                            (cNode.Row, cNode.Col - 1, 1, Direc.left),
                        ],
                    Direc.right =>
                        [
                            (cNode.Row - 1, cNode.Col, 1001, Direc.up),
                            (cNode.Row + 1, cNode.Col, 1001, Direc.down),
                            (cNode.Row, cNode.Col + 1, 1, Direc.right),
                        ]
                };

                foreach ((int r, int c, int cost, Direc dir) in nextPos)
                {
                    if (RawInput[r][c] is '#' || visited.Contains((r, c, dir)))
                    {
                        continue;
                    }
                    Node newNode = new(r, c, cNode.G + cost, dir, cNode, endPos);
                    frontier.Enqueue(newNode, newNode.F);
                }
            }

            return endNode.G.ToString();
        }

        public string Part2(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            (int r, int c) startPos = (-1, -1);
            (int r, int c) endPos = (-1, -1);
            for (int r = 0; r < RawInput.Length; r++)
            {
                for (int c = 0; c < RawInput[r].Length; c++)
                {
                    if (RawInput[r][c] is 'S')
                    {
                        startPos = (r, c);
                    }
                    else if (RawInput[r][c] is 'E')
                    {
                        endPos = (r, c);
                    }
                }
            }

            Node start = new(startPos.r, startPos.c, endPos);

            PriorityQueue<Node, int> frontier = new();
            frontier.Enqueue(start, 0);
            Dictionary<(int r, int c, Direc dir), Node> visited = [];

            Node endNode = null;
            while (frontier.Count > 0)
            {
                Node cNode = frontier.Dequeue();
                if (visited.TryGetValue((cNode.Row, cNode.Col, cNode.Dir), out Node vNode))
                {
                    if (vNode.G == cNode.G)
                    {
                        vNode.PrevNodes = vNode.PrevNodes.Union(cNode.PrevNodes).ToHashSet();
                        cNode.PrevNodes = vNode.PrevNodes;
                    }

                    continue;
                }
                visited[(cNode.Row, cNode.Col, cNode.Dir)] = cNode;

                if (cNode.Row == endPos.r && cNode.Col == endPos.c && (endNode is null || endNode.G >= cNode.G))
                {
                    endNode = cNode;
                }

                List<(int r, int c, int cost, Direc dir)> nextPos = cNode.Dir switch
                {
                    Direc.up =>
                        [
                            (cNode.Row - 1, cNode.Col, 1, Direc.up),
                            (cNode.Row, cNode.Col - 1, 1001, Direc.left),
                            (cNode.Row, cNode.Col + 1, 1001, Direc.right)
                        ],
                    Direc.down =>
                        [
                            (cNode.Row + 1, cNode.Col, 1, Direc.down),
                            (cNode.Row, cNode.Col - 1, 1001, Direc.left),
                            (cNode.Row, cNode.Col + 1, 1001, Direc.right)
                        ],
                    Direc.left =>
                        [
                            (cNode.Row - 1, cNode.Col, 1001, Direc.up),
                            (cNode.Row + 1, cNode.Col, 1001, Direc.down),
                            (cNode.Row, cNode.Col - 1, 1, Direc.left),
                        ],
                    Direc.right =>
                        [
                            (cNode.Row - 1, cNode.Col, 1001, Direc.up),
                            (cNode.Row + 1, cNode.Col, 1001, Direc.down),
                            (cNode.Row, cNode.Col + 1, 1, Direc.right),
                        ]
                };

                foreach ((int r, int c, int cost, Direc dir) in nextPos)
                {
                    if (RawInput[r][c] is '#')
                    {
                        continue;
                    }
                    Node newNode = new(r, c, cNode.G + cost, dir, cNode, endPos);
                    frontier.Enqueue(newNode, newNode.F);
                }
            }

            Queue<Node> toUnwrap = new();
            toUnwrap.Enqueue(endNode);
            HashSet<(int r, int c)> optimalPos = [];
            while (toUnwrap.Count > 0)
            {
                Node next = toUnwrap.Dequeue();
                optimalPos.Add((next.Row, next.Col));
                foreach (Node p in next.PrevNodes)
                {
                    toUnwrap.Enqueue(p);
                }
            }

            return optimalPos.Count.ToString();
        }
    }
}