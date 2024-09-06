using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class RunJacamo : MonoBehaviour
{
    private Process jacamoProcess;
    public GameObject[] avatars;
    public GameObject[] environmentArtifacts;

    async void Start()
    {
        // Define agents into the multi agent system
        UnityJacamoIntegrationUtil.configureJcmFile(avatars);

        print(".jcm file configuration done successfully.");

        // Run jacamo application in async
        await RunJaCaMoApp();        

        // Start to connect each avatar
        startWebSocketConnections();
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
            jacamoProcess.StartInfo.UseShellExecute = false;
            jacamoProcess.StartInfo.RedirectStandardOutput = true; // Capture output
            jacamoProcess.StartInfo.RedirectStandardError = true;
            jacamoProcess.StartInfo.CreateNoWindow = true; // Hide the command window

            try
            {
                // Start the process
                jacamoProcess.Start();
                print("Jacamo application started. Waiting for it to fully launch...");
                string output = "";
                // Read the output to determine if Jacamo has successfully started
                while (!jacamoProcess.HasExited) // Keep reading while the process is running
                {
                    output = jacamoProcess.StandardOutput.ReadLine();
                    if (!string.IsNullOrEmpty(output))
                    {
                        print("Jacamo Output: " + output);

                        // Check for the signal from the .bat file
                        if (output.Contains("JACAMO_LAUNCH_SUCCESSFUL"))
                        {
                            print("Jacamo application is up and running.");
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                print("Error starting Jacamo application: " + ex.Message);
            }
            
        });
    }

    private void startWebSocketConnections()
    {
        // Start avatar web socket connections
        foreach (GameObject avatar in avatars)
        {
            AvatarScript avatarScript = avatar.GetComponent<AvatarScript>();

            if (avatarScript == null)
            {
                throw new Exception("The avatar" + avatar.name + " has not script Avatar");
            }

            avatarScript.connectWs();
        }

        // Start web socket connections
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
}