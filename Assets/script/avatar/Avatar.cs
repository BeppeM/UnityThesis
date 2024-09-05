using System;
using UnityEngine;
using WebSocketSharp;

public class Avatar : MonoBehaviour
{
    public GameObject avatar;
    private WebSocketChannel webSocketChannel;
    public string port;

    void Start()
    {
        // Initialize new web socket connection
        string url = "ws://localhost:" + port;
        WSConnectionInfoModel wSConnectionInfoModel = new WSConnectionInfoModel(url, "AGENT", avatar.name);
        webSocketChannel = new WebSocketChannel(wSConnectionInfoModel, OnMessage);
    }

    // Check if agent has reached the destination
    public void CheckDestinationReached()
    {
        avatar.GetComponent<ReachDestination>().stopWalking();
        // Set the next destination
        sendMessageToJaCaMo("entered_into_supermarket");
    }

    private void sendMessageToJaCaMo(string actionToPerform)
    {
        WsMessage wsMessage = PrepareMessageUtil.prepareMessage(null, actionToPerform, "all");
        string jsonString = JsonUtility.ToJson(wsMessage);
        Debug.Log(wsMessage.getActionToPerform());
        Debug.Log(jsonString);
        webSocketChannel.sendMessage(jsonString);
    }

    // Unity avatar receives message from jacamo agent
    private void OnMessage(object sender, MessageEventArgs e)
    {
        string data = e.Data;
        Debug.Log("Received message: " + data);

        if (data == "supermarket_door_opened") // Let the agent reach the destination
        {
            // Dispatch the move action to the main thread
            UnityMainThreadDispatcher.Instance()
            .Enqueue(() =>
            {              
                avatar.GetComponent<ReachDestination>().reachDestination("SupermarketDoor");
            });

        }
        else if (data == "supermarket_door_closed") // Stop the agent
        {
            // Dispatch the move action to the main thread
            UnityMainThreadDispatcher.Instance()
            .Enqueue(() => avatar.GetComponent<ReachDestination>().stopWalking());
        }
        else if (data == "reach_fruit_seller")
        {
            // Dispatch the move action to the main thread
            UnityMainThreadDispatcher.Instance()
            .Enqueue(() =>
            {              
                avatar.GetComponent<ReachDestination>().reachDestination("FruitSeller");
            });
        }
    }
}
