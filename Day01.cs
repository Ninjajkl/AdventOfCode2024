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

            foreach (var line in RawInput)
            {
                Match match = Regex.Match(line, pattern);

                long left = long.Parse(match.Groups[1].Value);
                long right = long.Parse(match.Groups[2].Value);
                group1.Enqueue(left, left);
                group2.Enqueue(right, right);
            }

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

            List<long> group1 = new();
            Dictionary<long, int> group2 = new(); 

            foreach (var line in RawInput)
            {
                Match match = Regex.Match(line, pattern);

                long left = long.Parse(match.Groups[1].Value);
                long right = long.Parse(match.Groups[2].Value);
                group1.Add(left);
                if (group2.ContainsKey(right))
                {
                    group2[right] += 1;
                }
                else
                {
                    group2.Add(right, 1);
                }
            }

            long similarityScore = 0;

            foreach (long i in group1)
            {
                if (group2.ContainsKey(i))
                {
                    similarityScore += group2[i] * i;
                }
            }

            return similarityScore.ToString();
        }

    }
}
