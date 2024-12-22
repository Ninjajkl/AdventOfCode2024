namespace AdventOfCode2024
{
    internal class Day22
    {
        public string Part1(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            long sum = 0;
            foreach (string stringBuyer in RawInput)
            {
                long buyer = long.Parse(stringBuyer);
                sum += CalcSecretNumber(buyer, 2000);
            }
            return sum.ToString();
        }

        public long CalcSecretNumber(long secret, int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                mix(64*secret, ref secret);
                prune(ref secret);
                mix(secret/32, ref secret);
                prune(ref secret);
                mix(2048*secret, ref secret);
                prune(ref secret);
            }
            return secret;
        }

        public void mix(long givenValue, ref long secret)
        {
            secret ^= givenValue;
        }

        public void prune(ref long secret)
        {
            secret %= 16777216;
        }

        public string Part2(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            Dictionary<(int, int, int, int), int[]> monkeyMarket = [];
            for (int i = 0; i < RawInput.Length; i++)
            {
                long buyer = long.Parse(RawInput[i]);
                CalcSecretMarket(buyer, 2000, monkeyMarket, i, RawInput.Length);
            }

            return monkeyMarket.Max(kv => kv.Value.Sum()).ToString();
        }

        public long CalcSecretMarket(long secret, int iterations, Dictionary<(int, int, int, int), int[]> monkeyMarket, int monkeyID, int buyersCount)
        {
            int prevOffer;
            Queue<int> differenceQueue = new();

            for (int i = 0; i < iterations; i++)
            {
                prevOffer = (int)(secret % 10);

                mix(64*secret, ref secret);
                prune(ref secret);
                mix(secret/32, ref secret);
                prune(ref secret);
                mix(2048*secret, ref secret);
                prune(ref secret);

                int currOffer = (int)(secret % 10);
                differenceQueue.Enqueue(currOffer-prevOffer);
                if (differenceQueue.Count > 4)
                {
                    differenceQueue.Dequeue();
                }
                if (differenceQueue.Count == 4)
                {
                    var tuple = differenceQueue.Take(4).ToList();
                    (int, int, int, int) key = (tuple[0], tuple[1], tuple[2], tuple[3]);
                    if (monkeyMarket.TryGetValue(key, out int[] monkeyOffers))
                    {
                        if (monkeyOffers[monkeyID] != 0)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        monkeyMarket[key] = new int[buyersCount];
                    }
                    monkeyMarket[key][monkeyID] = currOffer;
                }
            }
            return secret;
        }
    }
}