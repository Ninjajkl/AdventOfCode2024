namespace AdventOfCode2024
{
    internal class Day12
    {
        public class Region
        {
            public bool Enabled { get; set; } = true;
            public int Perimeter { get; set; } = 0;
            public List<(int, int)> Plots { get; set; } = [];
        }
        public string Part1(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            Dictionary<(int, int), Region> plotMap = [];
            List<Region> regions = [];

            for (int r = 0; r < RawInput.Length; r++)
            {
                for (int c = 0; c < RawInput[r].Length; c++)
                {
                    char plot = RawInput[r][c];

                    bool abovePlot = false;
                    bool prevPlot = false;
                    if (r - 1 >= 0 && RawInput[r - 1][c] == plot)
                    {
                        abovePlot = true;
                    }
                    if (c - 1 >= 0 && RawInput[r][c - 1] == plot)
                    {
                        prevPlot = true;
                    }

                    if (abovePlot || prevPlot)
                    {
                        (int r, int c) aPlot = (r-1, c);
                        (int r, int c) pPlot = (r, c-1);
                        if (abovePlot && prevPlot)
                        {
                            Region region = plotMap[aPlot];
                            region.Plots.Add((r, c));
                            plotMap.Add((r, c), region);
                            if (plotMap[aPlot].Plots != plotMap[pPlot].Plots)
                            {
                                region.Perimeter += plotMap[pPlot].Perimeter;
                                plotMap[pPlot].Enabled = false;
                                foreach ((int, int) leftPlot in plotMap[pPlot].Plots)
                                {
                                    plotMap[leftPlot] = region;
                                    region.Plots.Add(leftPlot);
                                }
                            }
                        }
                        else if (abovePlot)
                        {
                            Region region = plotMap[aPlot];
                            region.Plots.Add((r, c));
                            region.Perimeter += 2;
                            plotMap.Add((r, c), region);
                        }
                        else
                        {
                            Region region = plotMap[pPlot];
                            region.Plots.Add((r, c));
                            region.Perimeter += 2;
                            plotMap.Add((r, c), region);
                        }
                    }
                    else
                    {
                        Region newRegion = new()
                        {
                            Perimeter = 4,
                            Plots = [(r, c)]
                        };
                        plotMap.Add((r, c), newRegion);
                        regions.Add(newRegion);
                    }
                }
            }
            return regions.Where(x => x.Enabled)
                .Select(x => x.Perimeter * x.Plots.Count)
                .Sum()
                .ToString();
        }

        public string Part2(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            Dictionary<(int, int), Region> plotMap = [];
            List<Region> regions = [];

            for (int r = 0; r < RawInput.Length; r++)
            {
                for (int c = 0; c < RawInput[r].Length; c++)
                {
                    char plot = RawInput[r][c];

                    bool abovePlot = false;
                    bool prevPlot = false;
                    if (r - 1 >= 0 && RawInput[r - 1][c] == plot)
                    {
                        abovePlot = true;
                    }
                    if (c - 1 >= 0 && RawInput[r][c - 1] == plot)
                    {
                        prevPlot = true;
                    }

                    if (abovePlot || prevPlot)
                    {
                        (int r, int c) aPlot = (r-1, c);
                        (int r, int c) pPlot = (r, c-1);
                        bool topLeft = c - 1 >= 0 && r - 1 >= 0 && RawInput[r - 1][c - 1] == plot;
                        bool topRight = c + 1 < RawInput[0].Length && r - 1 >= 0 && RawInput[r - 1][c + 1] == plot;
                        if (abovePlot && prevPlot)
                        {
                            Region region = plotMap[aPlot];
                            region.Plots.Add((r, c));
                            plotMap.Add((r, c), region);
                            if (plotMap[aPlot].Plots != plotMap[pPlot].Plots)
                            {
                                region = plotMap[aPlot];
                                region.Perimeter += plotMap[pPlot].Perimeter;
                                plotMap[pPlot].Enabled = false;
                                foreach ((int, int) leftPlot in plotMap[pPlot].Plots)
                                {
                                    plotMap[leftPlot] = region;
                                    region.Plots.Add(leftPlot);
                                }
                            }
                            if (!topRight)
                            {
                                region.Perimeter -= 2;
                            }
                        }
                        else if (abovePlot)
                        {
                            Region region = plotMap[aPlot];
                            region.Plots.Add((r, c));
                            if (topLeft && topRight)
                            {
                                region.Perimeter += 4;
                            }
                            else if (topLeft || topRight)
                            {
                                region.Perimeter += 2;
                            }
                            plotMap.Add((r, c), region);
                        }
                        else
                        {
                            Region region = plotMap[pPlot];
                            region.Plots.Add((r, c));
                            if (topLeft)
                            {
                                region.Perimeter += 2;
                            }
                            plotMap.Add((r, c), region);
                        }
                    }
                    else
                    {
                        Region newRegion = new()
                        {
                            Perimeter = 4,
                            Plots = [(r, c)]
                        };
                        plotMap.Add((r, c), newRegion);
                        regions.Add(newRegion);
                    }
                }
            }
            return regions.Where(x => x.Enabled)
                .Select(x => x.Perimeter * x.Plots.Count)
                .Sum()
                .ToString();
        }
    }
}