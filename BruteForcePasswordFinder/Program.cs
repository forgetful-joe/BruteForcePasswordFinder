global using System;
global using System.Collections.Generic;
global using System.Linq;
global using BruteForcePasswordFinder.Helpers;
global using BruteForcePasswordFinder.Services;
global using BruteForcePasswordFinder.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;

namespace BruteForcePasswordFinder;

public class Program
{
    static PasswordService passwordService;
    static SecretCombinationService combinationService;
    static TimedAction globalTimer;

    static void Main(string[] args)
    {
        globalTimer = new TimedAction($"Running application on {Helper.MachineName()} on {Environment.ProcessorCount} threads");

        AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);


        try
        {
            SetupBasics(args);
            Process();
        }
        catch (Exception ex)
        {
            Helper.Write($"Unhandled exception at {DateTime.Now}: {ex.Message}", 0);
            Helper.Write(ex.ToString(), 0);

            throw;
        }
        finally
        {
            // shutting down computer never worked properly, so it has been commented
            // ShutdownComputer();
        }
    }

    public static void ShutdownComputer()
    {
        var psi = new ProcessStartInfo("shutdown", "/s /t 0");
        psi.CreateNoWindow = true;
        psi.UseShellExecute = false;
        System.Diagnostics.Process.Start(psi);

        Task.Delay(10000).Wait();
    }

    public static void Process()
    {
        if (FinishOustanding())
        {
            return;
        }

        combinationService.ProcessFiles();

        if (FinishOustanding())
        {
            return;
        }
    }

    public static void SetupBasics(string[] args)
    {
        passwordService = new PasswordService();
        combinationService = new SecretCombinationService();
        State.CurrentlyRunning.ReadLast();
        
        if (args.Length == 0)
        {
            // run the files found in the folder 'files'

            var fileNames = Directory.GetFiles("files", "*.json");
            if (fileNames.Count() == 0) throw new Exception("No json files found in folder 'files'");

            foreach (var filename in fileNames)
                State.Files.Add(Path.GetFileName(filename));
            
        }
        else
        {
            foreach (var arg in args) // run the files send in arguments
            {
                State.Files.Add(arg);
            }
        }
    }

    static void CurrentDomain_ProcessExit(object sender, EventArgs e)
    {
        globalTimer.Dispose();

        Helper.Write("", 0); // empty line
    }

    private static bool FinishOustanding()
    {
        return passwordService.FindPwd();
    }
}