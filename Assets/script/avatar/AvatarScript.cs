using System;
using UnityEngine;
using WebSocketSharp;

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
        sendMessageToJaCaMo("entered_into_supermarket");
    }

    private void sendMessageToJaCaMo(string actionToPerform)
    {
        WsMessage wsMessage = UnityJacamoIntegrationUtil.prepareMessage(null, actionToPerform, "all");
        string jsonString = JsonUtility.ToJson(wsMessage);
        print(wsMessage.getActionToPerform());
        print(jsonString);
        wsChannel.sendMessage(jsonString);
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
}
