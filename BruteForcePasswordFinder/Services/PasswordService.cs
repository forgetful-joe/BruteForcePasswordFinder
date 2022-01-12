using Humanizer;
using System.Threading;
using System.Threading.Tasks;

namespace BruteForcePasswordFinder.Services;

public class PasswordService
{
    private static ParallelOptions parallel;

    public PasswordService()
    {
        parallel = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
    }

    public bool FindPwd()
    {
        var outstandingFiles = State.Files
            .SelectMany(c => OutstandingPassword.ReadOustanding(c))
            .OrderBy(c => c.Priority)
            .ToList();

        if (!outstandingFiles.Any())
        {
            Helper.Write($"No outstanding password files", 0);
            return false;
        }

        var lastRunning = outstandingFiles.FirstOrDefault(c => State.CurrentlyRunning.IsSame(c));

        if (lastRunning != null)
        {
            Helper.Write($"Will continue running {State.CurrentlyRunning.FilePath} from line {State.CurrentlyRunning.LastLineRead.ToString("N0")}", 0);
            // make sure it's the first file
            outstandingFiles.Remove(lastRunning);
            outstandingFiles.Insert(0, lastRunning);
        }

        foreach (var item in outstandingFiles)
            if (FindPwd(item))
                return true;

        return false;

    }
    private bool FindPwd(OutstandingPassword outstandingPassword)
    {
        var keyStoreFacade = new KeyStoreFacade(outstandingPassword.KeyStoreFile.Content);

        var outstanding = outstandingPassword.GetPasswords();

        uint rowIndex = 0;

        if (State.CurrentlyRunning.IsSame(outstandingPassword))
            if (State.CurrentlyRunning.LastLineRead > 0)
            {
                rowIndex = State.CurrentlyRunning.LastLineRead;
                outstanding.DequeueChunk(rowIndex); // skip what we already tried            
            }

        double total = outstanding.Count();

        Helper.Write($"Begin trying {total.ToString("N0")} new passwords from {outstandingPassword} (like {outstanding.GetRandom(5).Stringfy()})", 0);

        State.CurrentlyRunning.SetFile(outstandingPassword.FileName);

        var watch = new System.Diagnostics.Stopwatch();

        watch.Start();

        var reportingInterval = (uint)parallel.MaxDegreeOfParallelism * 60;

        var pagedNewPasswords = outstanding.DequeueChunk(reportingInterval).ToList();

        CorrectPassword correctPwd = null;

        while (pagedNewPasswords.Count() > 0)
        {
            Parallel.ForEach(pagedNewPasswords, parallel,
                            (possiblePassword) =>
                            {
                                Interlocked.Increment(ref rowIndex);

                                if (keyStoreFacade.Decrypt(possiblePassword))
                                {
                                    correctPwd = possiblePassword;
                                    return;
                                }
                            });

            if (correctPwd)
            {
                outstandingPassword.KeyStoreFile.SaveSuccess(correctPwd);
                Helper.Write($"Your password from {outstandingPassword.KeyStoreFile.FileName} is [{correctPwd}]", MessageType.Success, 1);
                return true;
            }

            var percentage = Math.Round(rowIndex / total * 100, 1);
            var remainingTime = TimeSpan.FromSeconds(watch.Elapsed.TotalSeconds / (rowIndex+1) * (total - rowIndex));
            Helper.Write($"Progress {percentage}% from {outstandingPassword}, estimated finish in {remainingTime.Humanize(precision: 2)}, example: {pagedNewPasswords.Last()}", 1);

            outstandingPassword.KeyStoreFile.AddTriedPasswords(pagedNewPasswords);

            State.CurrentlyRunning.SetLastLineRead(rowIndex);

            pagedNewPasswords = outstanding.DequeueChunk(reportingInterval).ToList();
        }

        watch.Stop();

        outstandingPassword.Clear();
        State.CurrentlyRunning.Clear();

        Helper.Write($"Sorry, password not found from {outstandingPassword} after searching for {watch.Elapsed.Humanize(precision: 2)}", MessageType.Error);

        return false;
    }
}

