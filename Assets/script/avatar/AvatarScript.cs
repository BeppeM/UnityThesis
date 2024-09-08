using System;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.AI;
using Unity.VisualScripting;


public class AvatarScript : MASAbstract
{
    void Start()
    {
        initializeWebSocketConnection(OnMessage);
    }

    // Check if agent has reached the destination
    public void CheckDestinationReached()
    {
        objInUse.GetComponent<ReachDestination>().stopWalking();
        // Set the next destination
        WsMessage wsMessage = UnityJacamoIntegrationUtil.prepareMessage(null, "entered_into_supermarket", "all");
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
                objInUse.GetComponent<ReachDestination>().reachDestination("SupermarketDoor");
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
        }
    }
}
