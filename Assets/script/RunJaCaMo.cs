using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class RunJacamo : MonoBehaviour
{

    private Process jacamoProcess;
    private string jcmFilePath = "C:/Users/g.mirra/Desktop/supermarket/prova1.jcm";
    public GameObject[] avatars;
    public GameObject[] environmentArtifacts;

    async void Start()
    {
        // Define agents into the multi agent system
        await addAgentsToJcmFile();

        // Run jacamo application in async
        await RunJaCaMoApp();

        // Wait for jacamo application to start in async
        await WaitForJacamoWindow();

        print("Jacamo application started.");

        // Start to connect each avatar
        foreach (GameObject avatar in avatars)
        {
            AvatarScript avatarScript = avatar.GetComponent<AvatarScript>();

            if (avatarScript == null)
            {
                throw new Exception("The avatar" + avatar.name + " has not script Avatar");
            }

            avatarScript.connectWs();
        }

        // Start to connect each environment artifact
        foreach (GameObject envArtifact in environmentArtifacts)
        {
            MASAbstract mASAbstract = envArtifact.GetComponent<MASAbstract>();
            if (mASAbstract == null)
            {
                throw new Exception("The artifact" + envArtifact.name + " has not script.");
            }

            mASAbstract.connectWs();
        }
    }

    public async Task addAgentsToJcmFile()
    {
        // Read the current content of the file
        string[] fileLines = File.ReadAllLines(jcmFilePath);

        // Create the new agent definition
        //     string newAgent = $@"
        // agent {agentName}: agent.asl {{
        //   goals: initializeAgent(""{artifactName}"", {port})
        //   join: w
        //   focus: w.smDoor 
        //          w.smCounter
        // }}";        

        // Find the last workspace or agent block and insert the new agent before the closing brace
        // for (int i = fileLines.Length - 1; i >= 0; i--)
        // {
        //     if (fileLines[i].Contains("}"))
        //     {
        //         // Insert the new agent just before the closing brace
        //         fileLines[i] = newAgent + Environment.NewLine + fileLines[i];
        //         break;
        //     }
        // }
        await Task.Run(() =>
        {
            string msg = "Prova1";
            // Write the updated content back to the file
            File.WriteAllText(jcmFilePath, msg);

            Console.WriteLine($"Done writing");
        });
    }

    public async Task RunJaCaMoApp()
    {
        string jacamoFolderPath = @"C:/Users/g.mirra/Desktop/supermarket";
        string gradleCommand = "gradlew -q --console=plain";

        await Task.Run(() =>
        {
            jacamoProcess = new Process();
            jacamoProcess.StartInfo.WorkingDirectory = jacamoFolderPath;
            jacamoProcess.StartInfo.FileName = "cmd.exe"; // For Windows
            jacamoProcess.StartInfo.Arguments = $"/c {gradleCommand}";
            jacamoProcess.StartInfo.UseShellExecute = false; // Needed for redirection
            jacamoProcess.StartInfo.CreateNoWindow = false; // Show the command window

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
        });
    }

    private async Task WaitForJacamoWindow()
    {
        await Task.Run(() =>
        {
            while (jacamoProcess != null && !jacamoProcess.HasExited)
            {
                jacamoProcess.Refresh(); // Refresh process information

                if (jacamoProcess.MainWindowHandle != IntPtr.Zero) // Check if the window is open
                {
                    print("Jacamo window is open.");
                    return; // Exit the loop once the window is open
                }

                // Wait a bit before checking again
                System.Threading.Thread.Sleep(500);
            }

            if (jacamoProcess == null || jacamoProcess.HasExited)
            {
                print("Jacamo process exited before the window could open.");
            }
        });
    }
}