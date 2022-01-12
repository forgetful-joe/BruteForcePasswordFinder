namespace BruteForcePasswordFinder.Services;

/// <summary>
/// This is not typical approach for repo, but for a small application like this, storing in memory is ok
/// and easier to have a central singleton like approach
/// so we don't have to pass classes around byRef
/// </summary>
public static class State
{
    public static List<KeyStoreFile> Files = new List<KeyStoreFile>();
    public static CurrentlyRunning CurrentlyRunning = new CurrentlyRunning();
}