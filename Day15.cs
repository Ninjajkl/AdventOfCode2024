namespace AdventOfCode2024
{
    internal class Day15
    {
        private readonly Dictionary<char, (int r, int c)> directionMap = new()
        {
            {'<', (0,-1)},
            {'^', (-1,0)},
            {'>', (0, 1)},
            {'v', (1, 0)}
        };

        public string Part1(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            int blankLineIndex = Array.IndexOf(RawInput, "");

            string[] firstSection = RawInput.Take(blankLineIndex).ToArray();
            int rows = firstSection.Length;
            int cols = firstSection[0].Length;
            char[,] warehouse = new char[rows, cols];
            (int r, int c) robPos = (-1, -1);
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < firstSection[r].Length; c++)
                {
                    warehouse[r, c] = firstSection[r][c];
                    if (firstSection[r][c] == '@')
                    {
                        robPos = (r, c);
                    }
                }
            }

            string instructions = string.Join("", RawInput.Skip(blankLineIndex+1));

            foreach (char instr in instructions)
            {
                (int r, int c) dir = directionMap[instr];
                (int r, int c) = (robPos.r + dir.r, robPos.c + dir.c);

                int boxesMove = 0;

                while (true)
                {
                    char space = warehouse[r, c];
                    if (space == 'O')
                    {
                        boxesMove++;
                        r += dir.r;
                        c += dir.c;
                    }
                    else if (space == '#')
                    {
                        break;
                    }
                    else
                    {
                        (r, c) = (robPos.r + dir.r, robPos.c + dir.c);
                        for (int i = 1; i <= boxesMove; i++)
                        {
                            warehouse[r + (dir.r * i), c + (dir.c * i)] = 'O';
                        }
                        warehouse[r, c] = '@';
                        warehouse[robPos.r, robPos.c] = '.';
                        robPos = (r, c);
                        break;
                    }
                }
            }

            long sum = 0;
            for (int i = 0; i < warehouse.GetLength(0); i++) // Loop through rows
            {
                for (int j = 0; j < warehouse.GetLength(1); j++) // Loop through columns
                {
                    if (warehouse[i, j] == 'O')
                    {
                        sum += (100 * i) + j;
                    }
                }
            }

            return sum.ToString();
        }

        public string Part2(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            int blankLineIndex = Array.IndexOf(RawInput, "");

            string[] firstSection = RawInput.Take(blankLineIndex).ToArray();
            int rows = firstSection.Length;
            int cols = firstSection[0].Length;
            char[,] warehouse = new char[rows, cols*2];
            (int r, int c) robPos = (-1, -1);
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < firstSection[r].Length; c++)
                {
                    char tile = firstSection[r][c];
                    if (tile is '#' or '.')
                    {
                        warehouse[r, c*2] = tile;
                        warehouse[r, (c*2)+1] = tile;
                    }
                    else if (tile is 'O')
                    {
                        warehouse[r, c*2] = '[';
                        warehouse[r, (c*2)+1] = ']';
                    }
                    else
                    {
                        warehouse[r, c*2] = '@';
                        warehouse[r, (c*2)+1] = '.';
                        robPos = (r, c*2);
                    }
                }
            }

            string instructions = string.Join("", RawInput.Skip(blankLineIndex+1));

            foreach (char instr in instructions)
            {

                (int r, int c) dir = directionMap[instr];
                (int r, int c) = (robPos.r + dir.r, robPos.c + dir.c);

                bool isHorizontal = dir.c != 0;

                char space = warehouse[r, c];
                List<(int r, int c)> unvisitedBoxes = [];

                if (space is '[' or ']')
                {
                    if (space is '[')
                    {
                        unvisitedBoxes.Add((r, c));
                    }
                    else
                    {
                        unvisitedBoxes.Add((r, c-1));
                    }
                }
                else if (space is '#')
                {
                    continue;
                }
                else
                {
                    warehouse[r, c] = '@';
                    warehouse[robPos.r, robPos.c] = '.';
                    robPos = (r, c);
                    continue;
                }

                List<(int r, int c)> boxLocs = [];
                bool foundWall = false;
                while (unvisitedBoxes.Count > 0)
                {
                    (int r, int c) next = unvisitedBoxes.First();
                    boxLocs.Add((next.r, next.c));
                    unvisitedBoxes.Remove(next);

                    List<(int r, int c)> posToCheck = isHorizontal ? dir.c == -1 ? [(next.r, next.c + dir.c)] : [(next.r, next.c + (dir.c * 2))] : [(next.r + dir.r, next.c), (next.r + dir.r, next.c + 1)];

                    foreach ((int r, int c) pos in posToCheck)
                    {
                        space = warehouse[pos.r, pos.c];
                        if (space is '[' or ']')
                        {
                            if (space is '[')
                            {
                                if (!unvisitedBoxes.Contains(pos))
                                {
                                    unvisitedBoxes.Add((pos.r, pos.c));
                                }
                            }
                            else
                            {
                                if (!unvisitedBoxes.Contains((pos.r, pos.c-1)))
                                {
                                    unvisitedBoxes.Add((pos.r, pos.c-1));
                                }
                            }
                        }
                        else if (space is '#')
                        {
                            foundWall = true;
                            break;
                        }
                    }
                    if (foundWall)
                    {
                        break;
                    }
                }
                if (foundWall)
                {
                    continue;
                }

                boxLocs.Reverse();
                foreach ((int r, int c) pos in boxLocs)
                {
                    if (dir.c == 1)
                    {
                        warehouse[pos.r + dir.r, pos.c + dir.c + 1] = warehouse[pos.r, pos.c + 1];
                        warehouse[pos.r + dir.r, pos.c + dir.c] = warehouse[pos.r, pos.c];
                    }
                    else
                    {
                        warehouse[pos.r + dir.r, pos.c + dir.c] = warehouse[pos.r, pos.c];
                        warehouse[pos.r + dir.r, pos.c + dir.c + 1] = warehouse[pos.r, pos.c + 1];
                        warehouse[pos.r, pos.c + 1] = '.';
                        if (!isHorizontal)
                        {
                            warehouse[pos.r, pos.c] = '.';
                        }
                    }
                }
                warehouse[r, c] = '@';
                warehouse[robPos.r, robPos.c] = '.';
                robPos = (r, c);
            }

            long sum = 0;
            for (int i = 0; i < warehouse.GetLength(0); i++)
            {
                for (int j = 0; j < warehouse.GetLength(1); j++)
                {
                    if (warehouse[i, j] == '[')
                    {
                        sum += (100 * i) + j;
                    }
                }
            }

            return sum.ToString();
        }
    }
}