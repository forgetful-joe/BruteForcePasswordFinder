using System.IO;

namespace BruteForcePasswordFinder.Models
{
    public class CurrentlyRunning
    {
        private static readonly string SerializedFile = "files/CurrentRunning.json";

        public string FilePath { get; set; } = String.Empty;
        public uint LastLineRead { get; set; } = 0;

        public void ReadLast()
        {
            if (!File.Exists(SerializedFile))
                return;

            var obj = File.ReadAllText(SerializedFile).FromJson<CurrentlyRunning>();
            FilePath = obj.FilePath;
            LastLineRead = obj.LastLineRead;
        }

        public bool Any()
        {
            return FilePath != String.Empty;
        }

        public bool IsSame(OutstandingPassword outstanding)
        {
            return FilePath == outstanding.FileName;
        }

        public void SetLastLineRead(uint lastLineRead)
        {
            LastLineRead = lastLineRead;
            SaveFile();
        }

        public void SetFile(string filePath)
        {
            FilePath = filePath;
            LastLineRead = 0;
            SaveFile();
        }

        private void SaveFile()
        {
            File.WriteAllTextAsync(SerializedFile, this.ToJson());
        }

        public void Clear()
        {
            FilePath = String.Empty;
            LastLineRead= 0;
        }
    }
}
