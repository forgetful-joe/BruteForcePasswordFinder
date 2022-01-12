using static BruteForcePasswordFinder.Helpers.SecretFragmentService;

namespace BruteForcePasswordFinder.Services;

public class SecretCombinationService
{
    public void ProcessFiles()
    {
        var combinations = new List<CombinationSet>();

        // you can add many combinations to this list, it will be executed in priority order
        combinations.AddRange(AddMeal());

        // this will transform combinations into permutations, and save to files, so we can start executing next
        SaveNewPermutations(combinations);
    }

    public static List<CombinationSet> AddMeal()
    {
        var separators = GetSeparators();
        var result = new List<CombinationSet>();
        var set = CombinationSet.Create();

        for (Priority priority = Priority.UltraHigh; priority >= Priority.UltraLow; priority--)
        {
            var salads = GetSalads(priority).SelectMany(c => c.GetLowerCase()).ToList();
            var mainmeals = GetMainMeal(priority).SelectMany(c => c.GetLowerCase()).ToList();

            var combinationSet = new CombinationSet($"Meals {priority}", Priority.High);

            set = CombinationSet.Create();
            set.Append(salads);
            set.Append(separators);
            set.Append(mainmeals);
            set.Append(separators);
            set.Append(GetNumbers(priority));
            combinationSet.Add(set);

            result.Add(combinationSet);
        }

        return result;
    }

    public void SaveNewPermutations(List<CombinationSet> combinations)
    {
        combinations = combinations.OrderByDescending(c => c.Priority).ToList();

        using (new TimedAction($"Looking for new permutations for {combinations.Count.ToString("N0")} combinations"))
            for (int i = 0; i < combinations.Count; i++)
            {
                combinations[i].SetAllPermutations();
                if (i > 0)
                {
                    var previous = combinations.Where((c, i2) => i2 < i).Select(c => c.AllPermutations);
                    combinations[i].AllPermutations.Remove(previous);
                }

                Helper.Write($"[{combinations[i].Name}] has {combinations[i].AllPermutations.Count.ToString("N0")} permutations found (like {combinations[i].AllPermutations.GetRandom(6).Stringfy()})", 1);
            }

        GC.Collect();

        foreach (var file in State.Files)
            file.ExcludeAlreadyTriedAndSave(combinations);

        combinations = null;

        GC.Collect();
    }

}
