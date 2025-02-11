using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Unity.VisualScripting;


class UnityJacamoIntegrationUtil : MonoBehaviour
{

    private static string jcmFilePath = "C:/Users/g.mirra/Desktop/supermarket/supermarket.jcm";
    private static string[] fileLines = {
        "mas supermarket {",
        "\tworkspace w {\n",
    "}"
    };

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
            AbstractArtifact script = envArtifact.GetComponent<AbstractArtifact>();
            print("Analize " + envArtifact.name); 
            print(" of type: " + script.ArtifactType);
            string artifact = "\t\t" + $@"artifact {envArtifact.name.FirstCharacterToLower()}: artifact.{script.ArtifactType.ToString()}Artifact({"\"" + envArtifact.name + "\""}, {script.Port}";
            if(script.ArtifactProperties != null){
                artifact += $@", ""{script.ArtifactProperties}"")";
            }else{                
                artifact += ")";
            }
            artifact += "\n";
            fileLines[1] += artifact;
        }

        fileLines[1] += "\t}\n";

        // Configure all agents
        foreach (GameObject avatar in avatars)
        {
            string artifactName = avatar.name + "Agent";
            AbstractAvatar avatarScript = avatar.GetComponent<AbstractAvatar>();

            // Create the new agent definition
            string goals = avatarScript.Goals != null
            ? $"initGoals([{string.Join(", ", avatarScript.Goals.Select(goal => goal.ToString().ToLower()))}])"
            : "";
            string beliefs = avatarScript.AgentBeliefs.GetBeliefsAsLiterals() != "" ? "beliefs:" + avatarScript.AgentBeliefs.GetBeliefsAsLiterals() : "";
            string newAgent = $@"
    agent {avatar.name}: {avatarScript.AgentFile} {{
        {beliefs}
        goals: initializeAgent({artifactName}, ""{avatarScript.JaCaMoAgentClassPath}"" , {avatarScript.port}, {goals})
        join: w";
            // Define focus on artifacts
            string artifactsFocused = "\t\t" + $@"focus:";            
            foreach (GameObject art in avatarScript.FocusedArtifacts)
            {
                artifactsFocused += $@" w.{art.name.FirstCharacterToLower()}";
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
    public static async Task RunJaCaMoApp(string jacamoFolderPath)
    {
        Process jacamoProcess;         
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
            AbstractAvatar avatarScript = avatar.GetComponent<AbstractAvatar>();

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
            AbstractArtifact artifactScript = envArtifact.GetComponent<AbstractArtifact>();
            if (artifactScript == null)
            {
                throw new Exception("The artifact" + envArtifact.name + " has not script.");
            }

            tasks.Add(artifactScript.connectWs());
        }

        await Task.WhenAll(tasks);
    }

    public static string createAndConvertJacamoMessageIntoJsonString(string messageType, string messagePayload, string agentEvent, string agentName, object param)
    {
        WsMessage wsMessage = new WsMessage(messageType, messagePayload, agentEvent, agentName, param);
        return JsonConvert.SerializeObject(wsMessage);
    }
}