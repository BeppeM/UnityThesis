using System;
using UnityEngine;
using WebSocketSharp;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Unity.VisualScripting;

public class AvatarScript : AbstractAvatar
{
    private void Awake()
    {
        agentFile = "shopper.asl";
        initializeWebSocketConnection(OnMessage);
    }

    // Unity avatar receives message from jacamo agent
    private void OnMessage(object sender, MessageEventArgs e)
    {
        string data = e.Data;
        print("Received message: " + data);
        AgentMessage message = null;
        //TODO: REMOVE ME!!
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
            string payload = message.Payload;
            if (payload == "stop_walking")
            {
                UnityMainThreadDispatcher.Instance()
                .Enqueue(() =>
                {
                    objInUse.GetComponent<ReachDestination>().stopWalking();
                });
                return;
            }
            if (payload == "exitDoor")
            {
                UnityMainThreadDispatcher.Instance()
                .Enqueue(() =>
                {
                    objInUse.GetComponent<ReachDestination>().reachDestination("exitDoor");
                });
                return;
            }
            // Avatar receives the type of artifact to reach
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {                
                objInUse.GetComponent<ReachDestination>().reachDestination(payload);
            });
        }
        catch (Exception ex)
        {
            print("Exception occoured OnMessage " + ex);
        }
    }

    // When Player enters into supermarket
    void OnTriggerEnter(Collider other)
    {
        // reached_destination(destName)
        if (!other.gameObject.name.Contains("counter") && other.gameObject.tag == "Artifact")
        {                     
            wsChannel.sendMessage(UnityJacamoIntegrationUtil.createAndConvertJacamoMessageIntoJsonString(objInUse.name, "signal_agent", 
                "reached_destination", other.name.FirstCharacterToLower()));          
        }
        if (other.gameObject.name.Contains("exitDoor"))
        {
            wsChannel.sendMessage(UnityJacamoIntegrationUtil.createAndConvertJacamoMessageIntoJsonString(objInUse.name,
                "signal_agent", "reached_destination", other.name.ToLowerInvariant()));            
            return;
        }
    }
}
