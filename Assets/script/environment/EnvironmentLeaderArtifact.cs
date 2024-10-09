using Newtonsoft.Json;
using System.Linq;
using System;
using UnityEngine;
using WebSocketSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EnvironmentLeader : AbstractArtifact
{

    void Awake()
    {
        Type = ArtifactTypeEnum.EnvLeader;
        // Initialize new web socket connection
        initializeWebSocketConnection(OnMessage);
    }

    private void OnMessage(object sender, MessageEventArgs e)
    {
        string data = e.Data;
        print("Received message: " + data);
        EnvLeaderMsg message = null;
        try
        {
            message = JsonConvert.DeserializeObject<EnvLeaderMsg>(data);
        }
        catch (Exception)
        {
            print("Message could not be converted.");
            return;
        }
        try
        {
            string actionType = message.ActionType;
            switch (actionType)
            {
                case "all_artifact_by_type": // Retrieve all artifacts by the type
                    retrieveArtifactsByType(message.ResourceType);
                    break;
                case "all_artifact":
                    retrieveAllArtifacts();
                    break;
                case "nearest":
                    //TODO
                    break;

            }
        }
        catch (Exception ex)
        {
            print("Exception occoured OnMessage " + ex);
        }
    }

    private void retrieveAllArtifacts()
    {
        // Retrieve all artifacts in the game
        UnityMainThreadDispatcher.Instance()
        .Enqueue(() =>
        {
            GameObject[] artifacts = GameObject.FindGameObjectsWithTag("Artifact");
            string artifactNames = "[" + string.Join(", ", artifacts.Select(artifact => artifact.name)) + "]";
        });

    }

    private async void retrieveArtifactsByType(string resourceType)
    {        
        // Create a TaskCompletionSource to await the result
        TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();

        UnityMainThreadDispatcher.Instance()
        .Enqueue(() =>
        {
            // Retrieve all artifacts in the game
            GameObject[] artifacts = GameObject.FindGameObjectsWithTag("Artifact");
            // Find the first artifact with the given type
            List<GameObject> filteredArtifacts = artifacts
                .Where(artifact => artifact
                .GetComponent<AbstractArtifact>().Type.ToString() == resourceType)
                .ToList();

            string artifactNames = "[" + string.Join(", ", filteredArtifacts.Select(artifact => artifact.name)) + "]";
            tcs.SetResult(artifactNames);
        });

        string artifactNames = await tcs.Task;
        EnvLeaderMsgRequest msgRequest = new EnvLeaderMsgRequest();
        msgRequest.Action = "retrieve_artifact_info";
        msgRequest.ActionType = "all_artifact_by_type";

        wsChannel.sendMessage(artifactNames);
    }
}
