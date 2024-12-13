using System.Text.RegularExpressions;

namespace AdventOfCode2024
{
    internal class Day13
    {
        const string pattern = @"(\d+)";

        public string Part1(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            long credits = 0;
            for (int i = 0; i < RawInput.Length; i+=4)
            {
                List<int> m = Regex.Matches(RawInput[i]+RawInput[i+1]+RawInput[i+2], pattern).Select(x => int.Parse(x.Value)).ToList();
                int A1 = m[0];
                int A2 = m[1];
                int B1 = m[2];
                int B2 = m[3];
                int P1 = m[4];
                int P2 = m[5];

                float b = (A1 * P2 - A2 * P1) / (float)(B2 * A1 - A2 * B1);
                float a = (P1 - B1*b) / (float)A1;
                if (a != Math.Floor(a) || b != Math.Floor(b))
                {
                    continue;
                }
                credits += 3*(int)a + (int)b;
            }
            return credits.ToString();
        }

        public string Part2(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            long credits = 0;
            for (int i = 0; i < RawInput.Length; i+=4)
            {
                List<int> m = Regex.Matches(RawInput[i]+RawInput[i+1]+RawInput[i+2], pattern).Select(x => int.Parse(x.Value)).ToList();
                int A1 = m[0];
                int A2 = m[1];
                int B1 = m[2];
                int B2 = m[3];
                long P1 = m[4] + 10000000000000;
                long P2 = m[5] + 10000000000000;

                double b = (A1 * P2 - A2 * P1) / (double)(B2 * A1 - A2 * B1);
                double a = (P1 - B1*b) / (double)A1;
                if (a != Math.Floor(a) || b != Math.Floor(b))
                {
                    continue;
                }
                credits += 3*(long)a + (long)b;
            }
            return credits.ToString();
        }
    }
}