using Humanizer;
using System.IO;

namespace BruteForcePasswordFinder.Models
{
    public class KeyStoreFile
    {
        public string FileName;
        public string FolderName;
        protected string _fileNameAttemptedPasswords;
        public string Content { get; private set; }

        public KeyStoreFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentNullException(nameof(fileName));
            FileName = "files/" + fileName;
            if (!File.Exists(FileName)) throw new ArgumentException($"Could not find file '{FileName}'");
            Content = File.ReadAllText(FileName);
            FolderName = $"files/{Path.GetFileNameWithoutExtension(fileName)}/";
            Directory.CreateDirectory(Path.GetDirectoryName(FolderName));

            _fileNameAttemptedPasswords = $"{FolderName}tries_{Helper.MachineName()}_{DateTime.Now.ToString("yyyy-MM")}.txt";
        }

        internal void ExcludeAlreadyTriedAndSave(List<CombinationSet> combinations)
        {
            var triedPasswordFiles = Directory.GetFiles(FolderName, "tries*.txt", SearchOption.TopDirectoryOnly).ToList();

            var oustandingFiles = combinations
                .Select((c, i) => new OutstandingPassword((ushort)i, this, c))
                .ToList();

            RemoveAlreadyTriedPasswords(triedPasswordFiles, oustandingFiles);

            oustandingFiles.ForEach(c => c.Save());
        }

        private static ushort totalLines = 500;

        private void RemoveAlreadyTriedPasswords(List<string> triedPasswordFiles, List<OutstandingPassword> oustandingFiles)
        {
            using (new TimedAction($"Excluding already tried passwords for {this} from {triedPasswordFiles.Count} files"))
            {
                foreach (var triedPasswordFile in triedPasswordFiles)
                {
                    var size = new FileInfo(triedPasswordFile).Length.Bytes();
                    using (new TimedAction($"Excluding already tried passwords from {triedPasswordFile} ({size.Humanize("MB")})", 1))
                    using (FileStream fs = File.Open(triedPasswordFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (BufferedStream bs = new BufferedStream(fs))
                    using (StreamReader sr = new StreamReader(bs))
                    {
                        List<string> lines;
                        while ((lines = sr.ReadLines(totalLines)).Count > 0)
                            oustandingFiles.ForEach(c => c.AllPermutations.Remove(lines));
                    }
                }
                oustandingFiles.ForEach(c => Helper.Write($"{c} now has {c.AllPermutations.Count.ToString("N0")} permutations", 2));
            }
        }

        public void SaveSuccess(string correctPassword)
        {
            File.WriteAllText(FolderName + "success.txt", correctPassword);
        }

        public void AddTriedPasswords(IEnumerable<string> triedPasswords)
        {
            File.AppendAllLines(_fileNameAttemptedPasswords, triedPasswords);
        }

        public static implicit operator KeyStoreFile(string value)
        {
            if (value == null)
                return null;

            return new KeyStoreFile(value);
        }

        public static implicit operator string(KeyStoreFile obj) => obj.FileName;
        public override string ToString() => FileName;
    }
}
