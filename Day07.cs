namespace AdventOfCode2024
{
    internal class Day07
    {
        public string Part1(string inputName)
        {
            string[] RawInput = System.IO.File.ReadAllLines(inputName);

            long sum = 0;
            foreach (string line in RawInput)
            {
                List<long> values = line.Replace(":", "").Split(' ').Select(long.Parse).ToList();
                long targetNum = values[0];
                values.RemoveAt(0);
                long nextVal = values[0];
                values.RemoveAt(0);
                sum += checkNextOp(nextVal, values, targetNum) ? targetNum : 0;
            }
            return sum.ToString();
        }

        public string Part2(string inputName)
        {
            string[] RawInput = System.IO.File.ReadAllLines(inputName);

            long sum = 0;
            foreach (string line in RawInput)
            {
                List<long> values = line.Replace(":", "").Split(' ').Select(long.Parse).ToList();
                long targetNum = values[0];
                values.RemoveAt(0);
                long nextVal = values[0];
                values.RemoveAt(0);
                sum += checkNextOpPlus(nextVal, values, targetNum) ? targetNum : 0;
            }
            return sum.ToString();
        }

        public bool checkNextOp(long currVal, List<long> remainingValues, long targetVal)
        {
            if (remainingValues.Count == 0)
            {
                return currVal == targetVal;
            }

            long plus = currVal + remainingValues[0];
            long mult = currVal * remainingValues[0];

            List<long> nextValues = new(remainingValues);
            nextValues.RemoveAt(0);
            return checkNextOp(plus, nextValues, targetVal) || checkNextOp(mult, nextValues, targetVal);
        }

        public bool checkNextOpPlus(long currVal, List<long> remainingValues, long targetVal)
        {
            if (remainingValues.Count == 0)
            {
                return currVal == targetVal;
            }

            long plus = currVal + remainingValues[0];
            long mult = currVal * remainingValues[0];
            long concat = long.Parse(currVal.ToString() + remainingValues[0].ToString());

            List<long> nextValues = new(remainingValues);
            nextValues.RemoveAt(0);
            return checkNextOpPlus(plus, nextValues, targetVal) || checkNextOpPlus(mult, nextValues, targetVal) || checkNextOpPlus(concat, nextValues, targetVal);
        }
    }
}