using System;
using UnityEngine;
using WebSocketSharp;

public class UnityWebSocketChannel : MonoBehaviour
{
    public GameObject avatar;
    private WebSocketChannel webSocketChannel;

    void Start()
    {
        // Initialize new web socket connection
        WSConnectionInfoModel wSConnectionInfoModel = new WSConnectionInfoModel("ws://localhost:8887", "AGENT");
        webSocketChannel = new WebSocketChannel(wSConnectionInfoModel, OnMessage);
    }

    private void OnMessage(object sender, MessageEventArgs e)
    {
        string data = e.Data;
        Debug.Log("Received message: " + data);
        if (data == "reachDest")
        {
            Debug.Log("I'm in");
            // Dispatch the move action to the main thread
            UnityMainThreadDispatcher.Instance()
            .Enqueue(() => avatar.GetComponent<ReachDestination>().reachDestination("blue"));
        }
        else if (data == "reachWhiteDest")
        {
            Debug.Log("I'm in");
            // Dispatch the move action to the main thread
            UnityMainThreadDispatcher.Instance()
            .Enqueue(() => avatar.GetComponent<ReachDestination>().reachDestination("white"));
        }

    }
}
