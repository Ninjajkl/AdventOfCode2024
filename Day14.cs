using System.Text.RegularExpressions;

namespace AdventOfCode2024
{
    internal class Day14
    {
        const string pattern = @"(-?\d+)";
        public string Part1(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            const int steps = 100;
            int height = RawInput.Count() < 20 ? 7 : 101;
            int width = RawInput.Count() < 20 ? 11 : 103;

            long q1 = 0;
            long q2 = 0;
            long q3 = 0;
            long q4 = 0;

            foreach (string line in RawInput)
            {
                List<int> m = Regex.Matches(line, pattern).Select(x => int.Parse(x.Value)).ToList();
                (int x, int y) pos = (m[0], m[1]);
                (int vx, int vy) vel = (m[2], m[3]);

                int nextX = (((pos.x + vel.vx * steps) % height) + height) % height;
                int nextY = (((pos.y + vel.vy * steps) % width) + width) % width;

                switch ((nextX, nextY))
                {
                    case var _ when nextX == height/2 || nextY == width/2:
                        break;
                    case var _ when nextX < height/2 && nextY < width/2:
                        q1++;
                        break;
                    case var _ when nextX > height/2 && nextY < width/2:
                        q2++;
                        break;
                    case var _ when nextX < height/2 && nextY > width/2:
                        q3++;
                        break;
                    default:
                        q4++;
                        break;
                }
            }
            return (q1*q2*q3*q4).ToString();
        }

        public string Part2(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            int height = RawInput.Count() < 20 ? 11 : 101;
            int width = RawInput.Count() < 20 ? 7 : 103;

            int steps = -1;
            bool check = false;
            while (!check)
            {
                steps++;
                check = false;
                List<int> xValues = [];
                List<int> yValues = [];
                foreach (string line in RawInput)
                {
                    List<int> m = Regex.Matches(line, pattern).Select(x => int.Parse(x.Value)).ToList();
                    (int x, int y) pos = (m[0], m[1]);
                    (int vx, int vy) vel = (m[2], m[3]);

                    int nextX = (((pos.x + vel.vx * steps) % height) + height) % height;
                    int nextY = (((pos.y + vel.vy * steps) % width) + width) % width;

                    xValues.Add(nextX);
                    yValues.Add(nextY);
                }

                double xVariance = Variance(xValues);
                if (xVariance < 700)
                {
                    double yVariance = Variance(yValues);
                    if (yVariance < 700)
                    {
                        check = true;
                    }
                }
            }
            return steps.ToString();
        }

        public static double Variance(List<int> values)
        {
            double mean = Mean(values);
            int start = 0;
            int end = values.Count;
            double variance = 0;

            for (int i = start; i < end; i++)
            {
                variance += Math.Pow((values[i] - mean), 2);
            }

            int n = end - start;
            if (start > 0) n -= 1;

            return variance / (n);
        }

        public static double Mean(List<int> values)
        {
            double s = 0;
            int start = 0;
            int end = values.Count();

            for (int i = start; i < end; i++)
            {
                s += values[i];
            }

            return s / (end - start);
        }
    }
}