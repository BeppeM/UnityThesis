using System;
using UnityEngine;
using WebSocketSharp;

public class UnityWebSocketChannel : MonoBehaviour
{
    private WebSocket ws;
    public GameObject avatar;

    void Start()
    {
        // Create a new WebSocket instance
        ws = new WebSocket("ws://localhost:8887");
        // Define the event handlers
        ws.OnOpen += OnOpen;
        ws.OnMessage += OnMessage;
        ws.OnClose += OnClose;
        ws.OnError += OnError;
        // Connect to the WebSocket server
        ws.Connect();
    }

    private void OnOpen(object sender, System.EventArgs e)
    {
        Debug.Log("WebSocket connection opened.");
        ws.Send("Connection established");
    }

    private void OnMessage(object sender, MessageEventArgs e)
    {
        String data = e.Data;
        Debug.Log("Received message: " + data);
        if (data == "reachDest")
        {
            Debug.Log("I'm in");
            // Dispatch the move action to the main thread
            UnityMainThreadDispatcher.Instance()
            .Enqueue(() => avatar.GetComponent<ReachDestination>().reachDestination());
        }
    }

    private void OnClose(object sender, CloseEventArgs e)
    {
        Debug.Log("WebSocket connection closed with reason: " + e.Reason);
    }

    private void OnError(object sender, ErrorEventArgs e)
    {
        Debug.LogError("WebSocket error: " + e.Message);
    }

    void OnDestroy()
    {
        // Clean up and close the WebSocket connection
        if (ws != null)
        {
            ws.Close();
            ws = null;
        }
    }
}
