using System;
using UnityEngine;
using WebSocketSharp;

public class UnityWebSocketChannel : MonoBehaviour
{
    public GameObject avatar;
    private WebSocketChannel webSocketChannel;
    public string port;

    void Start()
    {
        string url = "ws://localhost:" + port;
        // Initialize new web socket connection
        WSConnectionInfoModel wSConnectionInfoModel = new WSConnectionInfoModel(url, "AGENT", avatar.name);
        webSocketChannel = new WebSocketChannel(wSConnectionInfoModel, OnMessage);
    }

    private void OnMessage(object sender, MessageEventArgs e)
    {
        string data = e.Data;
        Debug.Log("Received message: " + data);
        if (data == "reachDest") // Let the agent reach the destination
        {
            // Dispatch the move action to the main thread
            UnityMainThreadDispatcher.Instance()
            .Enqueue(() => avatar.GetComponent<ReachDestination>().reachDestination("Door"));

        }
        else if (data == "stop") // Stop the agent
        {
            // Dispatch the move action to the main thread
            UnityMainThreadDispatcher.Instance()
            .Enqueue(() => avatar.GetComponent<ReachDestination>().stopWalking());
        }
    }
}
