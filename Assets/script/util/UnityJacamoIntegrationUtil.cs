using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;

class UnityJacamoIntegrationUtil
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

}