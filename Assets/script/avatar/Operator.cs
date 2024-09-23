using System;
using UnityEngine;
using WebSocketSharp;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

public class Operator : AbstractAvatar
{    

    void Start()
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
            string payload = message.Payload;
            CompleteTaskToPerformEnum task = (CompleteTaskToPerformEnum)Enum.Parse(typeof(CompleteTaskToPerformEnum), payload);
            string destination = "";
            switch (task)
            {
                case CompleteTaskToPerformEnum.reach_dress_shop:
                    destination = "DressShop";
                    break;
                case CompleteTaskToPerformEnum.reach_exit:
                    destination = "ExitDoor";
                    break;
                case CompleteTaskToPerformEnum.reach_fruit_seller:
                    destination = "FruitShop";
                    break;
                case CompleteTaskToPerformEnum.enter_into_supermarket:
                    destination = "Door";
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
    
}
