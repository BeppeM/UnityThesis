using System;
using UnityEngine;
using WebSocketSharp;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

public class AvatarScript : MASAbstract
{

    public List<TaskToPerformEnum> tasksToPerform;
    public GameObject[] focusedArtifacts;
    public InitialShopperAgentBeliefs initialShopperAgentBeliefs;

    void Start()
    {
        type = AgentArtifactTypeEnum.Agent;
        initializeWebSocketConnection(OnMessage);
    }

    public GameObject[] FocusedArtifacts
    {
        get { return focusedArtifacts; }
        set { focusedArtifacts = value; }
    }

    public List<TaskToPerformEnum> TasksToPerform
    {
        get { return tasksToPerform; }
    }

    // Check if agent has reached the destination
    public void CheckDestinationReached()
    {
        objInUse.GetComponent<ReachDestination>().stopWalking();
        string tasksString = "[" + string.Join(", ", tasksToPerform.Select(task => task.ToString())) + "]";

        // Set the next destination
        wsMessage = UnityJacamoIntegrationUtil.prepareMessage(null, "do_shopping", objInUse.name, tasksString);
        UnityJacamoIntegrationUtil.sendMessageToJaCaMo(wsMessage, wsChannel);
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
        }catch(Exception){
            print("Message could not be converted.");
            return;
        }
        string payload = message.Payload;
        if (payload == "supermarket_door_opened") // Let the agent reach the destination
        {
            // Dispatch the move action to the main thread
            UnityMainThreadDispatcher.Instance()
            .Enqueue(() =>
            {
                objInUse.GetComponent<ReachDestination>().reachDestination("Door");
            });
        }
        else if (payload == "supermarket_door_closed") // Stop the agent
        {
            // Dispatch the move action to the main thread
            UnityMainThreadDispatcher.Instance()
            .Enqueue(() => objInUse.GetComponent<ReachDestination>().stopWalking());
        }
        else
        {
            TaskToPerformEnum task = (TaskToPerformEnum)Enum.Parse(typeof(TaskToPerformEnum), payload);
            string destination = "";
            switch (task)
            {
                case TaskToPerformEnum.reach_dress_shop:
                    destination = "DressShop";
                    break;
                case TaskToPerformEnum.reach_exit:
                    destination = "ExitDoor";
                    break;
                case TaskToPerformEnum.reach_fruit_seller:
                    destination = "FruitSeller";
                    break;
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
    }

    // When Player enters into supermarket
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Door"))
        {
            CheckDestinationReached();
        }
        if (other.gameObject.name.Contains("FruitSeller"))
        {
            print(objInUse.name + " - Reached Fruit seller. Buy some fruits");
            // signal agents to buy some fruits
            wsMessage = UnityJacamoIntegrationUtil.prepareMessage(null, "fruit_seller_reached", objInUse.name, null);
            UnityJacamoIntegrationUtil.sendMessageToJaCaMo(wsMessage, wsChannel);
        }
        if (other.gameObject.name.Contains("DressShop"))
        {
            print(objInUse.name + " - Reached Dress shop. Buy some clothes");
            // signal agents to buy some fruits
            wsMessage = UnityJacamoIntegrationUtil.prepareMessage(null, "buy_clothes", objInUse.name, null);
            UnityJacamoIntegrationUtil.sendMessageToJaCaMo(wsMessage, wsChannel);
        }
        if (other.gameObject.name.Contains("ExitDoor"))
        {
            print(objInUse.name + " - Exited from the supermarket");
            // signal agents to buy some fruits
            wsMessage = UnityJacamoIntegrationUtil.prepareMessage(null, "exit", objInUse.name, null);
            UnityJacamoIntegrationUtil.sendMessageToJaCaMo(wsMessage, wsChannel);
        }
    }
}
