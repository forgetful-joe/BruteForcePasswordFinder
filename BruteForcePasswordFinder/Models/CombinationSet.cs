using System.Text.RegularExpressions;

namespace BruteForcePasswordFinder.Models
{
    public class CombinationSet
    {
        private static readonly Regex regex = new Regex("^[a-z0-9 ]{3,30}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public List<List<List<string>>> Combinations = new();
        public SortedSet<string> AllPermutations = new();


        public readonly string Name;
        public readonly Priority Priority;
        public readonly string[] MustContain = new string[0];
        

        public string PriorityAsNumber => ((int)Priority.UltraHigh - (int)Priority).ToString();


        public CombinationSet(string name, Priority priority, string[] mustContain = null)
        {
            if (!regex.IsMatch(name)) throw new ArgumentException("Name should be letter or space only");
            Name = name;
            this.Priority = priority;
            MustContain = mustContain ?? new string[0];
        }

        public static List<List<string>> Create()
        {
            return new List<List<string>>();
        }

        public void Add(List<List<string>> newItem)
        {
            Combinations.Add(newItem);
        }

        public void SetAllPermutations()
        {
            var permutations = Combinations.SelectMany(c => c.Permutate()).ToList();
            if (MustContain.Any())
                permutations.RemoveAll(c => !c.Contains(MustContain));

            foreach (var item in permutations
                                    .Where(c => c.MatchesPasswordRequirements())
                                    .Select(c => c.Trim())
                                    .Distinct())
                AllPermutations.Add(item);
        }

        public override string ToString()
        {
            return Name + " " + Priority;
        }
    }
}
