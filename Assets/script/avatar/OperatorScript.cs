using System;
using UnityEngine;
using WebSocketSharp;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Unity.VisualScripting;

public class OperatorScript : AbstractAvatar
{    
    void Awake()
    {
        agentFile = "operator.asl";
        initializeWebSocketConnection(OnMessage);
    }

    // Unity avatar receives message from jacamo agent
    private void OnMessage(object sender, MessageEventArgs e)
    {
        string data = e.Data;
        print("Received message: " + data);
        AgentMessage message = null;
        try
        {
            message = JsonConvert.DeserializeObject<AgentMessage>(data);
        }
        catch (Exception)
        {
            print("Message could not be converted.");
            return;
        }
        try
        {
            string destination = message.Payload;            
            // Dispatch the move action to the main thread
            UnityMainThreadDispatcher.Instance()
            .Enqueue(() =>
            {
                objInUse.GetComponent<ReachDestination>().reachDestination(destination);
            });
        }
        catch (Exception ex)
        {
            print("Exception occoured OnMessage " + ex);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // reached_destination(destName)
        if (other.gameObject.tag == "Artifact")
        {
            wsChannel.sendMessage(UnityJacamoIntegrationUtil.createAndConvertJacamoMessageIntoJsonString(objInUse.name, "signal_agent",
                "reached_destination", other.name.FirstCharacterToLower()));            
        }
    }

}
