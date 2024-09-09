using System;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.AI;
using Unity.VisualScripting;
using System.Collections.Generic;


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
        // Set the next destination
        wsMessage = UnityJacamoIntegrationUtil.prepareMessage(null, "entered_into_supermarket", objInUse.name);
        UnityJacamoIntegrationUtil.sendMessageToJaCaMo(wsMessage, wsChannel);
    }

    // Unity avatar receives message from jacamo agent
    private void OnMessage(object sender, MessageEventArgs e)
    {
        string data = e.Data;
        print("Received message: " + data);

        if (data == "supermarket_door_opened") // Let the agent reach the destination
        {
            // Dispatch the move action to the main thread
            UnityMainThreadDispatcher.Instance()
            .Enqueue(() =>
            {
                objInUse.GetComponent<ReachDestination>().reachDestination("Door");
            });

        }
        else if (data == "supermarket_door_closed") // Stop the agent
        {
            // Dispatch the move action to the main thread
            UnityMainThreadDispatcher.Instance()
            .Enqueue(() => objInUse.GetComponent<ReachDestination>().stopWalking());
        }
        else if (data == "reach_fruit_seller")
        {
            // Dispatch the move action to the main thread
            UnityMainThreadDispatcher.Instance()
            .Enqueue(() =>
            {
                objInUse.GetComponent<ReachDestination>().reachDestination("FruitSeller");
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
            print("Reached Fruit seller. Buy some fruits");
            // signal agents to buy some fruits
            // agent.isStopped = true;
        }
    }
}
