namespace AdventOfCode2024
{
    internal class Day09
    {
        public string Part1(string inputName)
        {
            string RawInput = System.IO.File.ReadAllLines(inputName)[0];

            List<int> convertedInt = RawInput
                .Select(x => x - '0')
                .ToList();

            List<(int value, int id)> files = convertedInt
                .Where((c, index) => index % 2 == 0)
                .Select((value, index) => (value, index))
                .ToList();

            List<int> freeSpace = convertedInt
                .Where((c, index) => index % 2 == 1)
                .ToList();

            long checkSum = 0;
            int currIndex = 0;

            bool isFile = true;
            while (files.Count > 0)
            {
                if (isFile)
                {
                    (int value, int id) currVal = files[0];
                    for (int j = 0; j < currVal.value; j++)
                    {
                        checkSum += currVal.id * currIndex++;
                    }
                    files.RemoveAt(0);
                    isFile = false;
                }
                else
                {
                    int remainingSpace = freeSpace[0];
                    for (int j = remainingSpace; j > 0; j--)
                    {
                        if (files.Count == 0)
                        {
                            break;
                        }
                        (int value, int id) currVal = files.Last();
                        checkSum += currVal.id * currIndex++;
                        files[^1] = new(currVal.value - 1, currVal.id);
                        if (currVal.value == 1)
                        {
                            files.RemoveAt(files.Count - 1);
                        }
                    }
                    freeSpace.RemoveAt(0);
                    isFile = true;
                }
            }

            return checkSum.ToString();
        }


        public string Part2(string inputName)
        {
            string RawInput = System.IO.File.ReadAllLines(inputName)[0];

            long runningTotal = 0;
            List<(int space, int id, long startIndex)> filesAndSpace = RawInput
                .Select((value, index) =>
                {
                    (int value, int id, long runningTotal) tuple = (value - '0', index, runningTotal);
                    runningTotal += value - '0';
                    return tuple;
                })
                .ToList();

            List<(int space, int id, long startIndex)> files = filesAndSpace
                .Where(tuple => tuple.id % 2 == 0)
                .Select(v => (v.space, v.id / 2, v.startIndex))
                .ToList();

            List<(int space, long startIndex)> freeSpace = filesAndSpace
                .Where(tuple => tuple.id % 2 == 1)
                .Select(v => (v.space, v.startIndex))
                .ToList();

            long checkSum = 0;
            for (int f = files.Count - 1; f >= 0; f--)
            {
                (int space, int id, long startIndex) = files[f];
                for (int i = 0; i < freeSpace.Count; i++)
                {
                    (int space, long startIndex) fSpace = freeSpace[i];
                    if (fSpace.space < space || fSpace.startIndex > startIndex) { continue; }

                    if (fSpace.space == space)
                    {
                        files[f] = (space, id, fSpace.startIndex);
                        startIndex = fSpace.startIndex;
                        freeSpace.RemoveAt(i);
                        break;
                    }
                    else
                    {
                        files[f] = (space, id, fSpace.startIndex);
                        startIndex = fSpace.startIndex;
                        freeSpace[i] = (fSpace.space - space, fSpace.startIndex + space);
                        break;
                    }
                }

                for (int s = 0; s < space; s++)
                {
                    checkSum += (startIndex + s) * id;
                }
            }

            return checkSum.ToString();
        }
    }
}