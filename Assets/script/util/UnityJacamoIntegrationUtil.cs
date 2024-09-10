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
        "\tworkspace w {\n",
    "}"
    };

    public static WsMessage prepareMessage(string environmentArtifactName, string actionToPerform, string agentInvolved)
    {
        return new WsMessage(environmentArtifactName, actionToPerform, agentInvolved);
    }

    //Utility used to configure .jcm file by adding agents 
    public static void ConfigureJcmFile(GameObject[] avatars, GameObject[] envArtifacts)
    {
        if (File.Exists(jcmFilePath))
        {
            File.Delete(jcmFilePath);
        }

        // Append HEADERS
        File.AppendAllText(jcmFilePath, fileLines[0]);

        // Configure artifacts
        foreach (GameObject envArtifact in envArtifacts)
        {
            string artifact = "\t\t" + $@"artifact {envArtifact.name.ToLowerInvariant()}: artifact.{envArtifact.GetComponent<MASAbstract>().Type}Artifact";
            artifact += "\n";
            fileLines[1] += artifact;
        }

        fileLines[1] += "}\n";

        // Configure all agents
        foreach (GameObject avatar in avatars)
        {
            string artifactName = avatar.name + "Artifact";
            AvatarScript avatarScript = avatar.GetComponent<AvatarScript>();

            // Create the new agent definition
            string goals = string.Join(", ", avatarScript.TasksToPerform);
            string newAgent = $@"
    agent {avatar.name}: agent.asl {{
        goals: initializeAgent({artifactName}, {avatarScript.port}), {goals}
        join: w";
            // Define focus on artifacts
            string artifactsFocused = "\t\t" + $@"focus:";            
            foreach (GameObject art in avatarScript.FocusedArtifacts)
            {
                artifactsFocused += $@" w.{art.name.ToLowerInvariant()}";
                artifactsFocused += "\n\t\t";
            }                                    
            newAgent += "\n" + artifactsFocused + "\n\t}";

            // Append into the file
            File.AppendAllText(jcmFilePath, newAgent + Environment.NewLine);
        }

        // FOOT
        File.AppendAllText(jcmFilePath, fileLines[1] + Environment.NewLine);
        File.AppendAllText(jcmFilePath, fileLines[2]);

    }

    // Open JaCaMo application
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

    // Starts web socket connections for avatars and environment objects 
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

    // Method to send message to jacamo artifacts
    public static void sendMessageToJaCaMo(WsMessage wsMessage, WebSocketChannel wsChannel)
    {
        string jsonString = JsonUtility.ToJson(wsMessage);
        print(wsMessage.getActionToPerform());
        print(jsonString);
        wsChannel.sendMessage(jsonString);
    }
}