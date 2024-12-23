namespace AdventOfCode2024
{
    internal class Day23
    {
        public string Part1(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            Dictionary<string, List<string>> compConnections = [];
            foreach (string input in RawInput)
            {
                (string c1, string c2) = (input.Substring(0, 2), input.Substring(3, 2));
                if (!compConnections.ContainsKey(c1))
                {
                    compConnections[c1] = [];
                }
                if (!compConnections.ContainsKey(c2))
                {
                    compConnections[c2] = [];
                }
                compConnections[c1].Add(c2);
                compConnections[c2].Add(c1);
            }

            HashSet<(string, string, string)> interConnected = [];
            foreach (string comp in compConnections.Keys)
            {
                if (comp[0] != 't')
                {
                    continue;
                }

                List<string> connected = compConnections[comp];
                foreach (string c1 in connected)
                {
                    List<string> c1Comps = compConnections[c1];
                    foreach (string c2 in c1Comps)
                    {
                        if (connected.Contains(c2))
                        {
                            interConnected.Add(new[] { comp, c1, c2 }
                                .OrderBy(s => s)
                                .ToArray() switch
                            {
                                var sorted => (sorted[0], sorted[1], sorted[2])
                            });
                        }
                    }
                }
            }
            return interConnected.Count.ToString();
        }

        public string Part2(string inputName)
        {
            string[] RawInput = File.ReadAllLines(inputName);

            Dictionary<string, List<string>> compConnections = [];
            foreach (string input in RawInput)
            {
                (string c1, string c2) = (input.Substring(0, 2), input.Substring(3, 2));
                if (!compConnections.ContainsKey(c1))
                {
                    compConnections[c1] = [];
                }
                if (!compConnections.ContainsKey(c2))
                {
                    compConnections[c2] = [];
                }
                compConnections[c1].Add(c2);
                compConnections[c2].Add(c1);
            }

            List<string> nodes = [.. compConnections.Keys];
            List<string> largestClique = [];

            Backtrack([], 0, nodes, ref largestClique, compConnections);

            largestClique.Sort();
            return string.Join(",", largestClique);
        }

        public static void Backtrack(List<string> currentClique, int index, List<string> nodes, ref List<string> largestClique, Dictionary<string, List<string>> compConnections)
        {
            if (currentClique.Count > largestClique.Count)
            {
                largestClique = new(currentClique);
            }

            for (int i = index; i < nodes.Count; i++)
            {
                string node = nodes[i];

                if (currentClique.All(c => compConnections[c].Contains(node)))
                {
                    currentClique.Add(node);
                    Backtrack(currentClique, i + 1, nodes, ref largestClique, compConnections);
                    currentClique.RemoveAt(currentClique.Count - 1);
                }
            }
        }
    }
}