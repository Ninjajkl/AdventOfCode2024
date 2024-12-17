using System.Text.RegularExpressions;

namespace AdventOfCode2024
{
    internal class Day17
    {
        private const string pattern = @"(-?\d+)";

        public string Part1(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            int A = int.Parse(Regex.Matches(RawInput[0], pattern)[0].Value);
            int B = int.Parse(Regex.Matches(RawInput[1], pattern)[0].Value);
            int C = int.Parse(Regex.Matches(RawInput[2], pattern)[0].Value);

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
                        int val = CO(operand, A, B, C) % 8;
                        output += output.Length == 0 ? val : "," + val;
                        instructionPointer += 2;
                        break;
                    case 6: //bdv
                        B = A / (int)Math.Pow(2, CO(operand, A, B, C));
                        instructionPointer += 2;
                        break;
                    case 7: //cdv
                        C = A / (int)Math.Pow(2, CO(operand, A, B, C));
                        instructionPointer += 2;
                        break;
                }
            }
            return output;
        }

        public string Part1Old(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            int A = int.Parse(Regex.Matches(RawInput[0], pattern)[0].Value);
            int B = int.Parse(Regex.Matches(RawInput[1], pattern)[0].Value);
            int C = int.Parse(Regex.Matches(RawInput[2], pattern)[0].Value);

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
                        int val = CO(operand, A, B, C) % 8;
                        output += output.Length == 0 ? val : "," + val;
                        instructionPointer += 2;
                        break;
                    case 6: //bdv
                        B = A / (int)Math.Pow(2, CO(operand, A, B, C));
                        instructionPointer += 2;
                        break;
                    case 7: //cdv
                        C = A / (int)Math.Pow(2, CO(operand, A, B, C));
                        instructionPointer += 2;
                        break;
                }
            }
            return output;
        }

        public string Part2(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            int value = 100;
            ref int v2 = ref value;
            v2 = 200;

            return value.ToString();
        }

        public int CO(int operand, int A, int B, int C)
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

        public readonly struct Range
        {
            public int Min { get; }
            public int Max { get; }

            public Range(int min, int max)
            {
                Min = min;
                Max = max;
            }

            public override string ToString()
            {
                return $"{Min} to {Max}";
            }
        }
    }
}