namespace AdventOfCode2024
{
    internal class Day06
    {
        public string Part1(string inputName)
        {
            string[] Map = System.IO.File.ReadAllLines(inputName);

            HashSet<(int r, int c)> rockLocations = [];
            (int r, int c) gL = (-1000, -1000);

            for (int r = 0; r < Map.Length; r++)
            {
                for (int c = 0; c < Map[r].Length; c++)
                {
                    if (Map[r][c] == '#')
                    {
                        rockLocations.Add((r, c));
                    }
                    else if (Map[r][c] == '^')
                    {
                        gL = (r, c);
                    }
                }
            }

            HashSet<(int, int)> visited = [gL];
            (int lon, int lat) dr = (-1, 0);

            while (true)
            {
                (int r, int c) nextStep = (gL.r + dr.lon, gL.c + dr.lat);
                if (nextStep.r < 0 ||
                    nextStep.r >= Map.Length ||
                    nextStep.c < 0 ||
                    nextStep.c >= Map[0].Length)
                {
                    break;
                }
                else if (!rockLocations.Contains(nextStep))
                {
                    visited.Add(nextStep);
                    gL = nextStep;
                }
                else
                {
                    dr = dr switch
                    {
                        (-1, 0) => (0, 1),
                        (0, 1) => (1, 0),
                        (1, 0) => (0, -1),
                        _ => (-1, 0)
                    };
                }
            }

            return visited.Count.ToString();
        }

        public string Part2(string inputName)
        {
            string[] Map = System.IO.File.ReadAllLines(inputName);

            HashSet<(int r, int c)> rockLocations = [];
            (int r, int c) gL = (-1000, -1000);

            for (int r = 0; r < Map.Length; r++)
            {
                for (int c = 0; c < Map[r].Length; c++)
                {
                    if (Map[r][c] == '#')
                    {
                        rockLocations.Add((r, c));
                    }
                    else if (Map[r][c] == '^')
                    {
                        gL = (r, c);
                    }
                }
            }
            (int r, int c) sP = gL;

            (int lon, int lat) dr = (-1, 0);
            HashSet<(int, int)> visited = [gL];

            HashSet<(int r, int c)> obstructionPos = [];

            while (true)
            {
                (int r, int c) nextStep = (gL.r + dr.lon, gL.c + dr.lat);
                if (nextStep.r < 0 ||
                    nextStep.r >= Map.Length ||
                    nextStep.c < 0 ||
                    nextStep.c >= Map[0].Length)
                {
                    break;
                }
                else if (!rockLocations.Contains(nextStep))
                {
                    if (!visited.Contains(nextStep) && willObstruct(Map, rockLocations, gL, dr))
                    {
                        obstructionPos.Add(nextStep);
                    }
                    visited.Add(nextStep);
                    gL = nextStep;
                }
                else
                {
                    dr = dr switch
                    {
                        (-1, 0) => (0, 1),
                        (0, 1) => (1, 0),
                        (1, 0) => (0, -1),
                        _ => (-1, 0)
                    };
                }
            }

            return obstructionPos.Count.ToString();
        }

        public bool willObstruct(string[] Map, HashSet<(int r, int c)> rL, (int r, int c) guardL, (int lon, int lat) dir)
        {
            (int r, int c) gL = guardL;
            HashSet<(int, int, int, int)> visited = [];
            HashSet<(int r, int c)> rockLocations = new(rL)
            {
                (gL.r + dir.lon, gL.c + dir.lat)
            };
            (int lon, int lat) dr = dir switch
            {
                (-1, 0) => (0, 1),
                (0, 1) => (1, 0),
                (1, 0) => (0, -1),
                _ => (-1, 0)
            };

            while (true)
            {
                (int r, int c) nextStep = (gL.r + dr.lon, gL.c + dr.lat);
                if (visited.Contains((nextStep.r, nextStep.c, dr.lon, dr.lat)))
                {
                    return true;
                }
                if (nextStep.r < 0 ||
                    nextStep.r >= Map.Length ||
                    nextStep.c < 0 ||
                    nextStep.c >= Map[0].Length)
                {
                    return false;
                }
                else if (!rockLocations.Contains(nextStep))
                {
                    visited.Add((nextStep.r, nextStep.c, dr.lon, dr.lat));
                    gL = nextStep;
                }
                else
                {
                    dr = dr switch
                    {
                        (-1, 0) => (0, 1),
                        (0, 1) => (1, 0),
                        (1, 0) => (0, -1),
                        _ => (-1, 0)
                    };
                }
            }
        }
    }
}