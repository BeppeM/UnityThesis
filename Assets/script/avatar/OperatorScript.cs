using System;
using UnityEngine;
using WebSocketSharp;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Unity.VisualScripting;
using TMPro;

public class OperatorScript : AbstractAvatar
{
    void Awake()
    {
        agentFile = "operator.asl";
        initializeWebSocketConnection(OnMessage);
        // Find the TextMeshPro component in the children of the avatar
        nameTextMeshPro = GetComponentInChildren<TextMeshPro>();

        // Check if we found the TextMeshPro component
        if (nameTextMeshPro != null)
        {
            // Set the text of the TextMeshPro to the avatar's name
            nameTextMeshPro.text = name;
        }
        else
        {
            Debug.LogWarning("TextMeshPro component not found in the avatar's children.");
        }
    }

    // Unity avatar receives message from jacamo agent
    private void OnMessage(object sender, MessageEventArgs e)
    {
        string data = e.Data;
        print("Received message: " + data);
        WsMessage message = null;
        try
        {
            message = JsonConvert.DeserializeObject<WsMessage>(data);
            switch (message.MessageType)
            {
                case "wsInitialization":
                    print("Connection established for " + objInUse.name);
                    break;
                case "reachDestination":
                    print("Agent needs to reach destination.");
                    // Avatar receives the type of artifact to reach
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        objInUse.GetComponent<ReachDestination>().reachDestination(message.MessagePayload);
                    });
                    break;
                default:
                    print("Unknown message type for " + objInUse.name);
                    break;
            }
        }
        catch (Exception)
        {
            print("Message could not be converted.");
            return;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // reached_destination(destName)
        if (other.gameObject.tag == "Artifact")
        {
            print("Agent " + objInUse.name + " reached destination " + other.name.FirstCharacterToLower());
            wsChannel.sendMessage(UnityJacamoIntegrationUtil.createAndConvertJacamoMessageIntoJsonString("destinationReached", null,
                "reached_destination", null, other.name.FirstCharacterToLower()));
        }
    }

}
