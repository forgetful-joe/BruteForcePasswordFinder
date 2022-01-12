using System.IO;
using System.Text.RegularExpressions;

namespace BruteForcePasswordFinder.Models
{
    public class OutstandingPassword
    {
        public SortedSet<string> AllPermutations = new();
        public string FileName = string.Empty;
        public UInt16 Index = 0;
        public UInt16 Priority = 0;
        public KeyStoreFile KeyStoreFile;

        public OutstandingPassword(UInt16 index, KeyStoreFile keyStoreFile, CombinationSet combinations)
        {
            Index = index;
            FileName = $"{keyStoreFile.FolderName}oustanding_{combinations.PriorityAsNumber}_{index}_{combinations.Name.Replace(" ", "")}.txt";
            AllPermutations = combinations.AllPermutations;
        }

        private static Regex filenameRegex = new Regex("oustanding_([0-9]+)_([0-9]+)", RegexOptions.Compiled);
        private OutstandingPassword(string fileName, KeyStoreFile keyStoreFile)
        {
            FileName = fileName;
            var match = filenameRegex.Match(FileName);
            if (!match.Success) throw new Exception($"Filename [{FileName}] doesn't have valid name (with number)");            
            Priority = UInt16.Parse(match.Groups[1].Value);
            Index = UInt16.Parse(match.Groups[2].Value);
            KeyStoreFile = keyStoreFile;                
        }


        public static List<OutstandingPassword> ReadOustanding(KeyStoreFile keyStoreFile)
        {
            var files = Directory.GetFiles(keyStoreFile.FolderName, "oustanding_*");

            if (!files.Any()) return new();

            var result = files.Select(c => new OutstandingPassword(c, keyStoreFile)).ToList();

            return result;
        }

        public Queue<string> GetPasswords()
        {
            return new Queue<string>(File.ReadAllLines(FileName));
        }

        public void Save()
        {
            if(AllPermutations.Any())
                File.AppendAllLines(FileName, AllPermutations);
        }

        internal void Clear()
        {           
            File.Delete(FileName);
        }

        public override string ToString()
        {
            return FileName;
        }
    }
}
