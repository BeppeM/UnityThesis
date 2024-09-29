using System;
using UnityEngine;
using WebSocketSharp;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

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
            if (payload == "reach_exit")
            {
                UnityMainThreadDispatcher.Instance()
                .Enqueue(() =>
                {
                    objInUse.GetComponent<ReachDestination>().reachDestination("exitDoor");
                });
                return;
            }
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                // Retrieve all artifacts in the game
                GameObject[] artifacts = GameObject.FindGameObjectsWithTag("Artifact");
                // Find the first artifact with the given type
                GameObject filteredArtifact = artifacts
                    .FirstOrDefault(artifact => artifact
                    .GetComponent<AbstractArtifact>().Type.ToString() == payload);
                if (filteredArtifact != null)
                {
                    objInUse.GetComponent<ReachDestination>().reachDestination(filteredArtifact.name);
                }
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
        if (other.gameObject.name.Contains("door"))
        {
            wsMessage = UnityJacamoIntegrationUtil.prepareMessage(null, "do_shopping", objInUse.name, null);
            UnityJacamoIntegrationUtil.sendMessageToJaCaMo(wsMessage, wsChannel);
            return;
        }
        if (!other.gameObject.name.Contains("counter") && other.gameObject.tag == "Artifact")
        {
            wsMessage = UnityJacamoIntegrationUtil.prepareMessage(null,"reached_destination",
                objInUse.name, other.name.ToLowerInvariant());

            UnityJacamoIntegrationUtil.sendMessageToJaCaMo(wsMessage, wsChannel);
            return;
        }
        if (other.gameObject.name.Contains("fruitShop"))
        {
            // signal agents to buy some fruits
            wsMessage = UnityJacamoIntegrationUtil.prepareMessage(null, "fruit_seller_reached", objInUse.name, null);
            UnityJacamoIntegrationUtil.sendMessageToJaCaMo(wsMessage, wsChannel);
        }
        if (other.gameObject.name.Contains("dressShop"))
        {
            // signal agents to buy some fruits
            wsMessage = UnityJacamoIntegrationUtil.prepareMessage(null, "dress_shop_reached", objInUse.name, null);
            UnityJacamoIntegrationUtil.sendMessageToJaCaMo(wsMessage, wsChannel);
        }
        if (other.gameObject.name.Contains("exitDoor"))
        {
            // signal agents to buy some fruits
            wsMessage = UnityJacamoIntegrationUtil.prepareMessage(null, "exit", objInUse.name, null);
            UnityJacamoIntegrationUtil.sendMessageToJaCaMo(wsMessage, wsChannel);
        }
    }
}
