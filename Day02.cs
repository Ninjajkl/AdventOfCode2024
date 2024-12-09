namespace AdventOfCode2024
{
    internal class Day02
    {
        /// <summary>
        /// Computes the answer for Part 1
        /// </summary>
        /// <param name="inputName">The address of the input file to take input from
        /// </param>
        /// <returns>
        /// Results the result as a string to be printed
        /// </returns>
        public string Part1(string inputName)
        {
            string[] RawInput = System.IO.File.ReadAllLines(inputName);
            return RawInput
                .Select(report => report.Split(' ')
                                        .Select(int.Parse)
                                        .ToList())
                .Count(levels => (levels.SequenceEqual(levels.OrderBy(n => n)) ||
                                 levels.SequenceEqual(levels.OrderByDescending(n => n))) &&
                                 levels.Zip(levels.Skip(1), (a, b) => Math.Abs(a - b))
                                        .All(diff => diff is >= 1 and <= 3))
                .ToString();
        }

        /// <summary>
        /// Computes the answer for Part 2
        /// </summary>
        /// <param name="inputName">The address of the input file to take input from
        /// </param>
        /// <returns>
        /// Results the result as a string to be printed
        /// </returns>
        public string Part2(string inputName)
        {
            string[] RawInput = System.IO.File.ReadAllLines(inputName);
            return RawInput
                .Select(report => report.Split(' ')
                                        .Select(int.Parse)
                                        .ToList())
                .Count(levels => Enumerable.Range(0, levels.Count)
                    .Any(i =>
                    {
                        List<int> subset = levels.Where((_, index) => index != i).ToList();
                        return (subset.SequenceEqual(subset.OrderBy(n => n)) ||
                              subset.SequenceEqual(subset.OrderByDescending(n => n))) &&
                             subset.Zip(subset.Skip(1), (a, b) => Math.Abs(a - b))
                                   .All(diff => diff is >= 1 and <= 3);
                    }))
                .ToString();
        }

    }
}
