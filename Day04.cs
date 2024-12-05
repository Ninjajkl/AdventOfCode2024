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
    internal class Day04
    {
        public string Part1(string inputName)
        {
            string[] RawInput = System.IO.File.ReadAllLines(inputName);

            int xmasTotal = 0;
            for (int r = 0; r < RawInput.Length; r++)
            {
                for (int c = 0; c < RawInput[r].Length; c++)
                {
                    if (RawInput[r][c] != 'X') { continue; }
                    for (int ra = -1; ra <= 1; ra++)
                    {
                        for (int ca = -1; ca <= 1; ca++)
                        {
                            if (ra * 3 + r < 0 || ra * 3 + r >= RawInput.Length) { continue; }
                            if (ca * 3 + c < 0 || ca * 3 + c >= RawInput[r].Length) { continue; }
                            if (ra == 0 && ca == 0) { continue; }

                            if (RawInput[ra + r][ca + c] == 'M' && 
                                RawInput[ra * 2 + r][ca * 2 + c] == 'A' &&
                                RawInput[ra * 3 + r][ca * 3 + c] == 'S') { xmasTotal++; }
                        }
                    }

                }
            }
            return xmasTotal.ToString();
        }

        public string Part2(string inputName)
        {
            string[] RawInput = System.IO.File.ReadAllLines(inputName);

            int xmasTotal = 0;
            for (int r = 0; r < RawInput.Length; r++)
            {
                for (int c = 0; c < RawInput[r].Length; c++)
                {
                    if (RawInput[r][c] != 'A') { continue; }
                    if (r - 1 < 0 || r + 1 >= RawInput.Length) { continue; }
                    if (c - 1 < 0 || c + 1 >= RawInput[r].Length) { continue; }

                    char[] chars = [RawInput[r - 1][c - 1], RawInput[r - 1][c + 1], RawInput[r + 1][c - 1], RawInput[r + 1][c + 1]];
                    
                    if (chars[0] != chars[3] &&
                        chars[1] != chars[2] &&
                        "MS".Contains(chars[0]) &&
                        "MS".Contains(chars[1]) &&
                        "MS".Contains(chars[2]) &&
                        "MS".Contains(chars[3]))
                    {
                        xmasTotal++;
                    }
                }
            }
            return xmasTotal.ToString();
        }
    }
}