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

    private static Dictionary<TaskToPerformEnum, ArtifactTypeEnum> artifactTypeFromTaskToPerform = new Dictionary<TaskToPerformEnum, ArtifactTypeEnum>()
        {
            { TaskToPerformEnum.reach_fruit_seller, ArtifactTypeEnum.FruitShop },
            { TaskToPerformEnum.reach_dress_shop, ArtifactTypeEnum.DressShop }
        };

    // Dictionary to map each action to perform for the shopper agent in each agent type to reach to achieve the goal
    public static Dictionary<TaskToPerformEnum, ArtifactTypeEnum> ArtifactTypeFromTaskToPerform
    {
        get { return artifactTypeFromTaskToPerform; }
    }

    public static MessageToSend prepareMessage(string environmentArtifactName, string actionToPerform, string agentInvolved, string param)
    {
        return new MessageToSend(environmentArtifactName, actionToPerform, agentInvolved, param);
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
            AbstractArtifact script = envArtifact.GetComponent<AbstractArtifact>();
            print("Analize " + envArtifact.name + " of type: " + script.Type);
            string artifact = "\t\t" + $@"artifact {envArtifact.name.ToLowerInvariant()}: artifact.{script.Type.ToString()}Artifact({"\"" + envArtifact.name + "\""}, {script.Port}";
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
            string newAgent = $@"
    agent {avatar.name}: {avatarScript.AgentFile} {{
        beliefs: {avatarScript.initialShopperAgentBeliefs.GetBeliefsString()}
        goals: initializeAgent({artifactName}, {avatarScript.port})
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
    public static void sendMessageToJaCaMo(MessageToSend wsMessage, WebSocketChannel wsChannel)
    {
        string jsonString = JsonUtility.ToJson(wsMessage);
        print(wsMessage.getActionToPerform());
        print(jsonString);
        wsChannel.sendMessage(jsonString);
    }
}