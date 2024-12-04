using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    internal class Day03
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
            string RawInput = string.Join("",System.IO.File.ReadAllLines(inputName));
            return Regex.Matches(RawInput, @"(?:mul\((\d*)\,(\d*)\))")
                .Cast<Match>()
                .Select(match => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value))
                .Sum()
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
        /// 
        public string Part2(string inputName)
        {
            string RawInput = "do()" + string.Join("", System.IO.File.ReadAllLines(inputName));
            string[] NewInput = RawInput.Split("don't()");
            long sum = 0;
            foreach (var i in NewInput)
            {
                int index = i.IndexOf("do()");
                if(index == -1)
                {
                    continue;
                }
                string substring = i.Substring(index);
                sum += Regex.Matches(substring, @"(?:mul\((\d*)\,(\d*)\))")
                .Cast<Match>()
                .Select(match => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value))
                .Sum();
            }
            return sum.ToString();
        }
    }
}
//do\(\).*?(?:mul\((\d*)\,(\d*)\))+
//do\(\).*?(?:mul\((\d*)\,(\d*)\)(?:(?!mul\(\d*\,\d*\)).)*)+
//do\(\)(?:(?!don't\(\)).)*?(?:mul\((\d*)\,(\d*)\)(?:(?!mul\(\d*\,\d*\)).)*)+
//do\(\)(?:(?!don't\(\)).)*?(?:mul\((\d*)\,(\d*)\)(?:(?!mul\(\d*\,\d*\)).)*(?:(?!don\'t\(\)).)*)+