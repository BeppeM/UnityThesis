using System;
using UnityEngine;
using WebSocketSharp;

public class UnityWebSocketChannel : MonoBehaviour
{
    public GameObject avatar;
    private WebSocketChannel webSocketChannel;
    public String port;

    void Start()
    {
        String url = "ws://localhost:" + port;
        // Initialize new web socket connection
        WSConnectionInfoModel wSConnectionInfoModel = new WSConnectionInfoModel(url, "AGENT", avatar.name);
        webSocketChannel = new WebSocketChannel(wSConnectionInfoModel, OnMessage);
    }

    private void OnMessage(object sender, MessageEventArgs e)
    {
        string data = e.Data;
        Debug.Log("Received message: " + data);
        if (data == "reachDest")
        {
            Debug.Log("I'm in");
            try
            {
                // // Dispatch the move action to the main thread
                UnityMainThreadDispatcher.Instance()
                .Enqueue(() => avatar.GetComponent<ReachDestination>().reachDestination("Door"));

            }
            catch (Exception ex)
            {
                Debug.LogError("Error: " + ex.Message);
            }
        } else if(data == "stop"){
                        try
            {
                // // Dispatch the move action to the main thread
                UnityMainThreadDispatcher.Instance()
                .Enqueue(() => avatar.GetComponent<ReachDestination>().stopWalking());

            }
            catch (Exception ex)
            {
                Debug.LogError("Error: " + ex.Message);
            }
        }
    }
}
