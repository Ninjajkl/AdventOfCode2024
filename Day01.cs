using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    internal class Day01
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
            string pattern = @"(\d+)\s+(\d+)";

            PriorityQueue<long, long> group1 = new();
            PriorityQueue<long, long> group2 = new();

            RawInput
                .Select(line => Regex.Match(line, pattern))
                .ToList()
                .ForEach(match =>
                {
                    long left = long.Parse(match.Groups[1].Value);
                    long right = long.Parse(match.Groups[2].Value);
                    group1.Enqueue(left, left);
                    group2.Enqueue(right, right);
                });

            long totalDifference = 0;

            while (group1.Count > 0)
            {
                totalDifference += Math.Abs(group1.Dequeue() - group2.Dequeue());
            }

            return totalDifference.ToString();
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
            string pattern = @"(\d+)\s+(\d+)";

            List<long> group1 = [];
            Dictionary<long, int> group2 = [];

            RawInput
                .Select(line => Regex.Match(line, pattern))
                .ToList()
                .ForEach(match =>
                {
                    long left = long.Parse(match.Groups[1].Value);
                    long right = long.Parse(match.Groups[2].Value);
                    group1.Add(left);
                    group2[right] = group2.GetValueOrDefault(right) + 1;
                });

            long similarityScore = group1
                .Where(i => group2.TryGetValue(i, out _))
                .Sum(i => group2[i] * i);

            return similarityScore.ToString();
        }

    }
}
