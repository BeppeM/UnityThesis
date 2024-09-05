using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;

public class RunJaCaMo : MonoBehaviour
{

    private Process jaCaMoProcess;

    async void Start()
    {
        await Task.Run(() => RunJaCaMoApp());
        print("CIAOAOAOAOAO");
    }

    public void RunJaCaMoApp()
    {
        string jacamoFolderPath = @"C:/Users/g.mirra/Desktop/supermarket";
        // jacamo command
        string gradleCommand = "gradlew -q --console=plain";

        Process jacamoProcess = new Process();

        jacamoProcess.StartInfo.WorkingDirectory = jacamoFolderPath;
        jacamoProcess.StartInfo.FileName = "cmd.exe"; // For Windows
        jacamoProcess.StartInfo.Arguments = $"/c {gradleCommand}";
        jacamoProcess.StartInfo.UseShellExecute = false; // Needed for redirection
        jacamoProcess.StartInfo.CreateNoWindow = true; // No command window

        try
        {
            // Start the process
            jacamoProcess.Start();
            print("Jacamo application started.");
        }
        catch (Exception ex)
        {
            print("Error starting Jacamo application: " + ex.Message);
        }
    }
}