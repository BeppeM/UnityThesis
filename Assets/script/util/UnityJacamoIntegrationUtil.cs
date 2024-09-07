using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;

class UnityJacamoIntegrationUtil : MonoBehaviour
{

    private static string jcmFilePath = "C:/Users/g.mirra/Desktop/supermarket/supermarket.jcm";
    private static string[] fileLines = {
        "mas supermarket {",
        @"    workspace w {
      artifact smDoor: artifact.SupermarketDoorArtifact()
      artifact smCounter: artifact.SupermarketCounterArtifact 
    }",
    "}"
    };

    public static WsMessage prepareMessage(string environmentArtifactName, string actionToPerform, string agentInvolved)
    {
        return new WsMessage(environmentArtifactName, actionToPerform, agentInvolved);
    }

    //Utility used to configure .jcm file by adding agents 
    public static void configureJcmFile(GameObject[] avatars)
    {

        if (File.Exists(jcmFilePath))
        {
            File.Delete(jcmFilePath);
        }

        // HEADERS
        File.AppendAllText(jcmFilePath, fileLines[0]);

        // Write all agents
        foreach (GameObject avatar in avatars)
        {
            string artifactName = avatar.name + "Artifact";

            // Create the new agent definition
            string newAgent = $@"
            agent {avatar.name}: agent.asl {{
            goals: initializeAgent(""{artifactName}"", {avatar.GetComponent<AvatarScript>().port})
            join: w
            focus: w.smDoor 
                   w.smCounter
            }}";

            File.AppendAllText(jcmFilePath, newAgent + Environment.NewLine);
        }

        // FOOT
        File.AppendAllText(jcmFilePath, fileLines[1] + Environment.NewLine);
        File.AppendAllText(jcmFilePath, fileLines[2]);
    }

    public static async Task RunJaCaMoApp()
    {
        Process jacamoProcess;
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
                string output = "";
                // Read the output to determine if Jacamo has successfully started
                while (!jacamoProcess.HasExited) // Keep reading while the process is running
                {
                    output = jacamoProcess.StandardOutput.ReadLine();
                    if (!string.IsNullOrEmpty(output))
                    {                        
                        // Check for the signal from the .bat file
                        if (output.Contains("JACAMO_LAUNCH_SUCCESSFUL"))
                        {
                            print("Jacamo Output: " + output);
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

    public static async Task StartWebSocketConnections(GameObject[] avatars, GameObject[] environmentArtifacts)
    {
        List<Task> tasks = new List<Task>();

        print("Connecting avatars...");
        // Start avatar web socket connections
        foreach (GameObject avatar in avatars)
        {
            AvatarScript avatarScript = avatar.GetComponent<AvatarScript>();

            if (avatarScript == null)
            {
                throw new Exception("The avatar" + avatar.name + " has not script Avatar");
            }

            tasks.Add(avatarScript.connectWs());
        }

        await Task.WhenAll(tasks);

        print("Connecting envArtifacts...");
        // Start web socket connections
        foreach (GameObject envArtifact in environmentArtifacts)
        {
            MASAbstract mASAbstract = envArtifact.GetComponent<MASAbstract>();
            if (mASAbstract == null)
            {
                throw new Exception("The artifact" + envArtifact.name + " has not script.");
            }

            tasks.Add(mASAbstract.connectWs());
        }

        await Task.WhenAll(tasks);
    }
}