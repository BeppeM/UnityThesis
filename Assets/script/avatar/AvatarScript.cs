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
        agentFile = "agent.asl";
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
            CompleteTaskToPerformEnum task = (CompleteTaskToPerformEnum)Enum.Parse(typeof(CompleteTaskToPerformEnum), payload);
            string destination = "";
            switch (task)
            {
                case CompleteTaskToPerformEnum.reach_dress_shop:
                    destination = "dressShop";
                    break;
                case CompleteTaskToPerformEnum.reach_exit:
                    destination = "exitDoor";
                    break;
                case CompleteTaskToPerformEnum.reach_fruit_seller:
                    destination = "fruitShop";
                    break;
                case CompleteTaskToPerformEnum.enter_into_supermarket:
                    destination = "door";
                    break;
                case CompleteTaskToPerformEnum.stop_walking:
                    UnityMainThreadDispatcher.Instance()
                    .Enqueue(() =>
                    {
                        objInUse.GetComponent<ReachDestination>().stopWalking();
                    });
                    return;
                default:
                    throw new Exception("Task cannot be performed");
            }
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

    // When Player enters into supermarket
    void OnTriggerEnter(Collider other)
    {        
        if (other.gameObject.name.Contains("door"))
        {
            wsMessage = UnityJacamoIntegrationUtil.prepareMessage(null, "do_shopping", objInUse.name, null);
            UnityJacamoIntegrationUtil.sendMessageToJaCaMo(wsMessage, wsChannel);
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
