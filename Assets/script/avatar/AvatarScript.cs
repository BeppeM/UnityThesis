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
    void Start()
    {
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
        // Assuming TaskToPerformEnum is your enum and you have a List<TaskToPerformEnum> called TasksToPerform
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
        AgentMessage message = JsonConvert.DeserializeObject<AgentMessage>(data);;
        string payload = message.Payload;
        print("Payload: " + payload);
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
        else if (payload.Contains("buy") || payload.Contains("exit"))
        {

            TaskToPerformEnum task = (TaskToPerformEnum)Enum.Parse(typeof(TaskToPerformEnum), payload);
            string destination = "";
            switch (task)
            {
                case TaskToPerformEnum.buy_clothes:
                    destination = "DressShop";
                    break;
                case TaskToPerformEnum.exit:
                    destination = "ExitDoor";
                    break;
                case TaskToPerformEnum.buy_fruit:
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
            wsMessage = UnityJacamoIntegrationUtil.prepareMessage(null, "next_task", objInUse.name, null);
            UnityJacamoIntegrationUtil.sendMessageToJaCaMo(wsMessage, wsChannel);
        }
        if (other.gameObject.name.Contains("DressShop"))
        {
            print("Reached Fruit seller. Buy some fruits");
            // signal agents to buy some fruits
            wsMessage = UnityJacamoIntegrationUtil.prepareMessage(null, "next_task", objInUse.name, null);
            UnityJacamoIntegrationUtil.sendMessageToJaCaMo(wsMessage, wsChannel);
        }
    }
}
