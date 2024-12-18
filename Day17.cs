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

            UnknownNumber A = new();
            A.SetToSingle(0);
            UnknownNumber B = new();
            UnknownNumber C = new();

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
                .Reverse()
                .ToArray();
            int instructionPointer = 0;
            int programIndex = 0;

            while (programIndex != program.Length || instructionPointer != 0)
            {
                (int instruction, int operand) = instructions[instructionPointer];

                long divisor = -1;
                switch (instruction)
                {
                    case 0: //adv
                        if (operand is >= 0 and <= 3)
                        {
                            divisor = operand;
                        }
                        divisor = (int)Math.Pow(2, divisor);

                        if (A.IsSingleNumber)
                        {
                            A.SetToRange(A.SingleVal * divisor, (A.SingleVal * divisor) + divisor - 1);
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                        break;
                    case 1: //bxl
                        B.SetToSingle(B.SingleVal ^ operand);
                        break;
                    case 2: //bst
                        ref UnknownNumber op = ref CO(operand, ref A, ref B, ref C);
                        if (op.IsRange)
                        {
                            long result = op.Min + B.SingleVal - (op.Min % 8);
                            if (result > op.Max)
                            {
                                throw new ArgumentOutOfRangeException(nameof(operand), "Invalid Value");
                            }
                            op.SetToSingle(result);
                            break;
                        }
                        break;
                    case 3: //jnz
                        if (A.IsRange && A.Min == 0)
                        {
                            A.Min = 1;
                        }
                        break;
                    case 4: //bxc
                        long roundInitB = programIndex == program.Length ? 0 : program[programIndex];
                        C.SetToSingle(B.SingleVal^5^1^roundInitB);
                        B.SetToSingle(B.SingleVal ^ C.SingleVal);
                        break;
                    case 5: //out
                        //Ignoring the cases where operand is 1,2,3 because that won't happen in a program bc it would just repeat the same output
                        ref UnknownNumber un = ref CO(operand, ref A, ref B, ref C);
                        int goalNum = program[programIndex++];

                        if (!un.Exists)
                        {
                            un.SetToSingle(goalNum);
                            break;
                        }
                        if (un.IsSingleNumber)
                        {
                            if (un.SingleVal % 8 != goalNum)
                            {
                                throw new ArgumentOutOfRangeException(nameof(operand), "Invalid Value");
                            }
                            break;
                        }
                        if (un.IsRange)
                        {
                            long result = un.Min + goalNum - (un.Min % 8);
                            if (result > un.Max)
                            {
                                throw new ArgumentOutOfRangeException(nameof(operand), "Invalid Value");
                            }
                            un.SetToSingle(result);
                            break;
                        }
                        break;
                    case 6: //bdv
                        if (operand is >= 0 and <= 3)
                        {
                            divisor = operand;
                        }
                        else
                        {
                            ref UnknownNumber combo = ref CO(operand, ref A, ref B, ref C);
                            if (!combo.Exists)
                            {
                                combo = new();
                                combo.SetToSingle(1);
                            }
                            if (combo.IsSingleNumber)
                            {
                                divisor = combo.SingleVal;
                            }
                        }
                        divisor = (int)Math.Pow(2, divisor);

                        if (A.IsSingleNumber)
                        {
                            B.SetToRange(A.SingleVal * divisor, (A.SingleVal * divisor) + divisor - 1);
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                        break;
                    case 7: //cdv
                        if (operand is >= 0 and <= 3)
                        {
                            divisor = operand;
                        }
                        else
                        {
                            ref UnknownNumber combo = ref CO(operand, ref A, ref B, ref C);
                            if (!combo.Exists)
                            {
                                combo = new();
                                combo.SetToSingle(1);
                            }
                            if (combo.IsSingleNumber)
                            {
                                divisor = combo.SingleVal;
                            }
                        }
                        divisor = (int)Math.Pow(2, divisor);

                        A.SetToRange(C.SingleVal * divisor, (C.SingleVal * divisor) + divisor - 1);

                        break;
                }
                instructionPointer++;
                instructionPointer %= instructions.Length;
            }
            return A.Min.ToString();
        }

        public long CO(int operand, long A, long B, long C)
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

        public ref UnknownNumber CO(int operand, ref UnknownNumber A, ref UnknownNumber B, ref UnknownNumber C)
        {
            switch (operand)
            {
                case 4:
                    return ref A;
                case 5:
                    return ref B;
                case 6:
                    return ref C;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operand), "Invalid operand");
            };
        }

        public struct UnknownNumber
        {
            public bool Exists { get; set; }
            public long Min { get; set; }
            public long Max { get; set; }
            public bool IsRange { get; set; }
            public long SingleVal { get; set; }
            public bool IsSingleNumber { get; set; }
            //public int Offset { get; set; }
            //public int Multiplier { get; set; }
            //public bool IsMultiple { get; set; }

            public UnknownNumber()
            {
                Min = -1;
                Max = -1;
                IsRange = false;
                Exists = false;
                SingleVal = -1;
                IsSingleNumber = false;
                //Offset = -1;
                //Multiplier = -1;
                //IsMultiple = false;
            }

            public void SetToRange(long min, long max)
            {
                Min = min;
                Max = max;
                IsRange = true;
                IsSingleNumber = false;
                Exists = true;
                //IsMultiple = false;
            }

            public void IncreaseMinimum(long min)
            {
                Min = min;
                if (Min == Max)
                {
                    SetToSingle(Min);
                }
            }

            public void DecreaseMaximum(long max)
            {
                Max = max;
                if (Max == Min)
                {
                    SetToSingle(Max);
                }
            }

            public void SetToSingle(long val)
            {
                SingleVal = val;
                IsRange = false;
                IsSingleNumber = true;
                Exists = true;
                //IsMultiple = false;
            }

            /*
            public void SetToMultiple(int offset, int multiplier)
            {
                Offset = offset;
                Multiplier = multiplier;
                IsRange = false;
                IsSingleNumber = false;
                IsMultiple = true;
            }
            */
        }
    }
}