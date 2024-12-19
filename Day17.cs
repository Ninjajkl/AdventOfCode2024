using System.Text.RegularExpressions;

namespace AdventOfCode2024
{
    internal class Day17
    {
        private const string pattern = @"(-?\d+)";

        public string Part1(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            long A = long.Parse(Regex.Matches(RawInput[0], pattern)[0].Value);
            long B = long.Parse(Regex.Matches(RawInput[1], pattern)[0].Value);
            long C = long.Parse(Regex.Matches(RawInput[2], pattern)[0].Value);

            int[] instructions = Regex.Matches(RawInput[4], pattern).Select(x => int.Parse(x.Value)).ToArray();
            int instructionPointer = 0;
            string output = "";

            while (instructionPointer < instructions.Length)
            {
                int instruction = instructions[instructionPointer];
                int operand = instructions[instructionPointer + 1];
                switch (instruction)
                {
                    case 0: //adv
                        A /= (int)Math.Pow(2, CO(operand, A, B, C));
                        instructionPointer += 2;
                        break;
                    case 1: //bxl
                        B ^= operand;
                        instructionPointer += 2;
                        break;
                    case 2: //bst
                        B = CO(operand, A, B, C) % 8;
                        instructionPointer += 2;
                        break;
                    case 3: //jnz
                        instructionPointer = A == 0 ? instructionPointer + 2 : operand;
                        break;
                    case 4: //bxc
                        B ^= C;
                        instructionPointer += 2;
                        break;
                    case 5: //out
                        long val = CO(operand, A, B, C) % 8;
                        output += output.Length == 0 ? val : "," + val;
                        instructionPointer += 2;
                        break;
                    case 6: //bdv
                        B = A / (long)Math.Pow(2, CO(operand, A, B, C));
                        instructionPointer += 2;
                        break;
                    case 7: //cdv
                        C = A / (long)Math.Pow(2, CO(operand, A, B, C));
                        instructionPointer += 2;
                        break;
                }
            }
            return output;
        }

        public string Part2(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            string stringOut = RawInput[4][9..];
            int[] program = stringOut
                .Split(',')
                .Select(int.Parse)
                .Reverse()
                .ToArray();

            (int instruction, int operand)[] instructions = program
                .Reverse()
                .Select((value, index) => new { value, index })
                .GroupBy(x => x.index / 2)
                .Select(g => (g.ElementAt(0).value, g.ElementAt(1).value))
                .ToArray();

            List<long> aList = [0];
            for (int p = 0; p < program.Length; p++)
            {
                List<long> newAList = [];
                foreach (long A in aList)
                {
                    long aMin = A * 8;
                    long aMax = (A * 8) + 7;
                    newAList.AddRange(SimulateRun(A, aMin, aMax, program[p], instructions));
                }
                aList = newAList;
            }

            return aList.Min().ToString();
        }

        public List<long> SimulateRun(long origA, long aMin, long aMax, long goalVal, (int instruction, int operand)[] instructions)
        {
            List<long> validA = [];
            for (long a = aMin; a <= aMax; a++)
            {
                long A = a;
                long B = 0;
                long C = 0;

                int instructionPointer = 0;
                bool isValid = true;
                bool breakEarly = false;
                while (instructionPointer < instructions.Length)
                {
                    (int instruction, int operand) = instructions[instructionPointer];
                    switch (instruction)
                    {
                        case 0: //adv
                            A /= (int)Math.Pow(2, CO(operand, A, B, C));
                            break;
                        case 1: //bxl
                            B ^= operand;
                            break;
                        case 2: //bst
                            B = CO(operand, A, B, C) % 8;
                            break;
                        case 3: //jnz
                            break;
                        case 4: //bxc
                            B ^= C;
                            break;
                        case 5: //out
                            long val = CO(operand, A, B, C) % 8;
                            if (!val.Equals(goalVal))
                            {
                                isValid = false;
                            }
                            breakEarly = true;
                            break;
                        case 6: //bdv
                            B = A / (long)Math.Pow(2, CO(operand, A, B, C));
                            break;
                        case 7: //cdv
                            C = A / (long)Math.Pow(2, CO(operand, A, B, C));
                            break;
                    }
                    if (breakEarly)
                    {
                        break;
                    }

                    instructionPointer++;
                }
                if (isValid && A/8 == origA)
                {
                    validA.Add(a);
                }
            }

            return validA;
        }

        public static long CO(int operand, long A, long B, long C)
        {
            return operand switch
            {
                0 => 0,
                1 => 1,
                2 => 2,
                3 => 3,
                4 => A,
                5 => B,
                6 => C,
                7 => throw new NotImplementedException()
            };
        }
    }
}