using Humanizer;
using System.IO;
using System.Text.Json;

namespace BruteForcePasswordFinder.Helpers;

public static class Extensions
{
    public static List<string> ReadLines(this StreamReader streamReader, ushort lines = 10)
    {
        var result = new List<string>(lines);
        string line;
        while ((line = streamReader.ReadLine()) != null && --lines > 0)
            result.Add(line);

        return result;
    }

    public static List<string> GetFirstHalf(this IEnumerable<string> initialList)
    {
        return initialList.Take(initialList.Count() / 2).ToList();
    }

    public static List<string> GetSecondHalf(this IEnumerable<string> initialList)
    {
        return initialList.Skip(initialList.Count() / 2).ToList();
    }

    public static string ToJson(this object obj)
    {
        return JsonSerializer.Serialize(obj);
    }

    public static T FromJson<T>(this string json)
    {
        return JsonSerializer.Deserialize<T>(json);
    }

    public static void Remove<T>(this SortedSet<T> main, IEnumerable<SortedSet<T>> others)
    {
        foreach (var other in others)
            main.Remove(other);
    }

    public static void Remove<T>(this SortedSet<T> main, SortedSet<T> other)
    {
        foreach (var item in other)
            main.Remove(item);
    }

    public static List<T> GetRandom<T>(this IEnumerable<T> l, int total = 3)
    {
        var result = new List<T>();

        if (l.Count() == 0) return result;

        var r = new Random();

        for (int i = 0; i < total; i++)
            result.Add(l.ElementAt(r.Next(l.Count())));

        return result;
    }

    public static List<string> GetAllCasePermutations(this string input)
    {
        var result = new List<string>();

        int n = input.Length;

        // Number of permutations is 2^n
        int max = 1 << n;

        // Converting string
        // to lower case
        input = input.ToLower();

        // Using all subsequences
        // and permuting them
        for (int i = 0; i < max; i++)
        {
            char[] combination = input.ToCharArray();

            // If j-th bit is set, we
            // convert it to upper case
            for (int j = 0; j < n; j++)
                if ((i >> j & 1) == 1)
                    combination[j] = (char)(combination[j] - 32);
            result.Add(new string(combination));
        }

        return result;
    }


    public static string Reverse(this string input)
    {
        char[] chars = input.ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }


    public static string[] GetLowerCase(this string word)
    {
        var result = new string[] {
                word, word.Replace(" ", ""),
                word.ToLower(), word.Replace(" ", "").ToLower()
            };
        return result.Distinct().ToArray();
    }



    public static bool Contains(this string word, params string[] options)
    {
        return options.Any(c => word.Contains(c, StringComparison.CurrentCultureIgnoreCase));
    }

    public static string[] GetPluralAndUpperCase(this string word)
    {
        var plural = word.Pluralize();
        var result = new string[] { word, plural, word.ToLower(), plural.ToLower(), word.ToUpper(), plural.ToUpper() };
        return result.Distinct().ToArray();
    }

    public static string[] GetUpperCase(this string word)
    {
        var firstLetter = word.First().ToString().ToUpper() + word.Substring(1);
        var result = new string[] { word, firstLetter };
        return result;
    }

    public static void AddReverse(this List<string> list)
    {
        list.AddRange(list.Select(c => c.Reverse()).ToArray());
        list.RemoveDuplicates();
    }

    public static void RemoveDuplicates<T>(this List<T> list)
    {
        var uniques = list.Distinct().ToArray();
        list.Clear();
        list.AddRange(uniques);
    }

    public static void Add(this List<string> list, params string[] strings)
    {
        foreach (var str in strings) list.Add(str);
    }


    public static void Append<T>(this List<List<T>> list, List<T> items, params T[] strings)
    {
        list.Add(items.ToList());
        list.Last().AddRange(strings);
    }

    public static void Append<T>(this List<List<T>> list, params T[] strings)
    {
        list.Add(new());
        list.Last().AddRange(strings);
    }

    public static void Append<T>(this List<List<T>> list, T item)
    {
        list.Add(new List<T>() { item });
    }

    public static List<string> Permutate(this List<List<string>> x, int a = 0)
    {
        // copied from https://stackoverflow.com/questions/710670/c-sharp-permutation-of-an-array-of-arraylists
        var retval = new List<string>();
        if (a == x.Count)
        {
            retval.Add("");
            return retval;
        }
        foreach (object y in x[a])
            foreach (string x2 in x.Permutate(a + 1))
                retval.Add(y + x2);

        var result = retval.Distinct().ToList();

        return result;
    }

    public static string FirstHalf(this string originalStr)
    {
        return originalStr.Substring(0, originalStr.Length / 2);
    }
    public static string SecondHalf(this string originalStr)
    {
        return originalStr.Substring(originalStr.Length / 2);
    }
    public static void ReplaceWith<T>(this List<T> list, IEnumerable<T> newItems)
    {
        list.Clear();
        list.AddRange(newItems);
    }

    public static bool MatchesPasswordRequirements(this string originalStr)
    {
        if (!(originalStr.Length >= 8 && originalStr.Any(char.IsLetter) && originalStr.Any(char.IsNumber)))
            return false;

        return true;
    }

    public static string Stringfy<T>(this List<T> list)
    {
        return string.Join(", ", list);
    }

    public static void Remove<T>(this SortedSet<T> set, IEnumerable<T> toRemove)
    {
        foreach (var remove in toRemove)
            set.Remove(remove);
    }

    public static IEnumerable<T> DequeueChunk<T>(this Queue<T> queue, uint chunkSize)
    {
        if (chunkSize <= 0) yield break;

        // copied from https://stackoverflow.com/a/33551927
        for (uint i = 0; i < chunkSize && queue.Count > 0; i++)
            yield return queue.Dequeue();
    }
}

