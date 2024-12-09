namespace AdventOfCode2024
{
    internal class Day05
    {
        public (Dictionary<int, List<int>>, List<List<int>>) GetData(string inputName)
        {
            string[] RawInput = System.IO.File.ReadAllLines(inputName);

            Dictionary<int, List<int>> pageOrderingRules = [];
            List<List<int>> updates = [];

            bool hitDivider = false;
            foreach (string l in RawInput)
            {
                if (l.Length == 0)
                {
                    hitDivider = true;
                    continue;
                }
                if (!hitDivider)
                {
                    int n1 = int.Parse(l[..2]);
                    int n2 = int.Parse(l[^2..]);
                    if (!pageOrderingRules.ContainsKey(n2))
                    {
                        pageOrderingRules[n2] = [];
                    }
                    pageOrderingRules[n2].Add(n1);
                }
                else
                {
                    updates.Add(l.Split(',').Select(int.Parse).ToList());
                }
            }
            return (pageOrderingRules, updates);
        }

        public string Part1(string inputName)
        {
            (Dictionary<int, List<int>>, List<List<int>>) inputs = GetData(inputName);

            Dictionary<int, List<int>> pageOrderingRules = inputs.Item1;
            List<List<int>> updates = inputs.Item2;

            int sum = 0;
            foreach (List<int> update in updates)
            {
                HashSet<int> mustBeBefore = [];
                bool isValid = true;
                foreach (int page in update)
                {
                    if (mustBeBefore.Contains(page))
                    {
                        isValid = false;
                        break;
                    }
                    mustBeBefore.UnionWith(pageOrderingRules[page]);
                }
                sum += isValid ? update[update.Count / 2] : 0;
            }
            return sum.ToString();
        }

        public string Part2(string inputName)
        {
            (Dictionary<int, List<int>>, List<List<int>>) inputs = GetData(inputName);

            Dictionary<int, List<int>> pageOrderingRules = inputs.Item1;
            List<List<int>> updates = inputs.Item2;

            int sum = 0;
            foreach (List<int> update in updates)
            {
                HashSet<int> mustBeBefore = [];
                bool isValid = true;
                foreach (int page in update)
                {
                    if (mustBeBefore.Contains(page))
                    {
                        isValid = false;
                        break;
                    }
                    mustBeBefore.UnionWith(pageOrderingRules[page]);
                }
                if (isValid)
                {
                    continue;
                }
                foreach (int page in update)
                {
                    if (pageOrderingRules[page].Intersect(update).Count() == update.Count / 2)
                    {
                        sum += page;
                        break;
                    }
                }
            }
            return sum.ToString();
        }
    }
}