using System;
using UnityEngine;
using WebSocketSharp;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

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
        if (!other.gameObject.name.Contains("counter") && !other.gameObject.name.Contains("door") && other.gameObject.tag == "Artifact")
        {
            wsMessage = UnityJacamoIntegrationUtil.prepareMessage(null, "reached_destination",
                objInUse.name, other.name.ToLowerInvariant());

            UnityJacamoIntegrationUtil.sendMessageToJaCaMo(wsMessage, wsChannel);
        }

        if (other.gameObject.name.Contains("exitDoor"))
        {
            wsMessage = UnityJacamoIntegrationUtil.prepareMessage(null, "reached_destination",
                objInUse.name, other.name.ToLowerInvariant());
            UnityJacamoIntegrationUtil.sendMessageToJaCaMo(wsMessage, wsChannel);
            return;
        }
    }

}
