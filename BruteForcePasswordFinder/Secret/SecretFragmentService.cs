namespace BruteForcePasswordFinder.Helpers;

public static partial class SecretFragmentService
{
    public static List<string> GetNumbersBetween(int a, int b)
    {
        var list = new List<string>() { "" };

        for (int i = a; i <= b; i++) list.Add(i.ToString());

        return list;
    }

    public static List<string> GetMainMeal(Priority? likelyhood = null)
    {
        var list = new List<string>();
        list.Add("");

        if (likelyhood == null || likelyhood <= Priority.UltraHigh)
        {
            list.Add("Pasta");
        }

        if (likelyhood == null || likelyhood <= Priority.High)
        {
            list.Add("Beef Steak");
            list.Add("Spaghetti");
        }

        if (likelyhood == null || likelyhood <= Priority.Medium)
        {
            list.Add("Rice");
        }

        return list.Distinct().ToList();
    }

    public static List<string> GetSeparators(Priority? likelyhood = null)
    {
        var list = new List<string>();
        list.Add("");

        if (likelyhood == null || likelyhood <= Priority.UltraHigh)
        {
            list.Add("+");
        }

        if (likelyhood == null || likelyhood <= Priority.High)
        {
            list.Add("and");
        }

        if (likelyhood == null || likelyhood <= Priority.Medium)
        {
            list.Add(" and ");
        }

        return list.Distinct().ToList();
    }

    public static List<string> GetNumbers(Priority? likelyhood = null)
    {
        var list = new List<string>();
        list.Add("");

        if (likelyhood == null || likelyhood <= Priority.UltraHigh)
        {
            list.Add("2007"); // first kid birth year
            list.Add("2013"); // second kid birth year
        }

        if (likelyhood == null || likelyhood <= Priority.VeryHigh)
        {
            list.AddRange(GetNumbersBetween(2005, 2010)); // years around the first kid birth year, in case I was being tricky
            list.AddRange(GetNumbersBetween(2012, 2015)); // years around the second kid birth year, in case I was being tricky
        }

        if (likelyhood == null || likelyhood <= Priority.High)
        {            
            list.Add("1981"); // my birth year
            list.Add("1983"); // wife birth year
        }

        if (likelyhood == null || likelyhood <= Priority.Medium)
        {

        }


        if (likelyhood == null || likelyhood <= Priority.Low)
        {

        }

        if (likelyhood == null || likelyhood <= Priority.VeryLow)
        {
            list.AddRange(GetNumbersBetween(1970, DateTime.Now.Year)); // desperate, don't know what else to try
        }

        return list.Distinct().ToList();
    }

    public static List<string> GetSalads(Priority? likelyhood = null, bool isExactSearch = false)
    {
        var list = new List<string>();
        list.Add("");

        if (likelyhood == null || (isExactSearch && likelyhood == Priority.UltraHigh) || (!isExactSearch && likelyhood <= Priority.UltraHigh))
        {
            list.Add("Lettuce", "Spinash"); // these are my favorites, so very likely to be correct
        }

        if (likelyhood == null || (isExactSearch && likelyhood == Priority.VeryHigh)  || (!isExactSearch && likelyhood <= Priority.VeryHigh))
        {
            list.Add("Brocolli", "Cauliflower"); // also like these, but not as much
        }

        if (likelyhood == null || (isExactSearch && likelyhood == Priority.High) || (!isExactSearch && likelyhood <= Priority.High))
        {
            list.Add("Cesar", "Vinagretti"); // feels unlikely, but just in case
        }

        if (likelyhood == null || (isExactSearch && likelyhood == Priority.Medium) || (!isExactSearch && likelyhood <= Priority.Medium))
        {
        }

        if (likelyhood == null || (isExactSearch && likelyhood == Priority.Low) || (!isExactSearch && likelyhood <= Priority.Low))
        {
        }

        if (likelyhood == null || (isExactSearch && likelyhood == Priority.VeryLow) || (!isExactSearch && likelyhood <= Priority.VeryLow))
        {
        }

        if (likelyhood == null || (isExactSearch && likelyhood == Priority.UltraLow) || (!isExactSearch && likelyhood <= Priority.UltraLow))
        {
            list.Add("Grub");
        }

        return list.Distinct().ToList();
    }
}
