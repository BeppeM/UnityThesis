// Define an interface named IDamageable
using System;
using System.Threading.Tasks;
using UnityEngine;

public abstract class MASAbstract : MonoBehaviour
{
    public GameObject objInUse;
    public string port;
    protected WebSocketChannel wsChannel;
    protected bool isConnected = false;

    protected void initializeWebSocketConnection(System.EventHandler<WebSocketSharp.MessageEventArgs> OnMessage)
    {
        print("Initializing connection for " + objInUse.name);
        // Initialize new web socket connection
        string url = "ws://localhost:" + port;
        WSConnectionInfoModel wSConnectionInfoModel = new WSConnectionInfoModel(url, "AGENT", objInUse.name);
        wsChannel = new WebSocketChannel(wSConnectionInfoModel, OnMessage);
    }

    // Method to connect to the websocket channel
    public async Task connectWs()
    {
        int maxRetryAttempt = 3;
        await Task.Run((Func<Task>)(async () =>
        {
            int currentAttempt = 1;
            while (!wsChannel.IsWebSocketConnected && currentAttempt < maxRetryAttempt)
            {
                print("Connecting attemp n°: " + currentAttempt);
                wsChannel.connect();
                if (wsChannel.IsWebSocketConnected)
                {
                    break;
                }
                currentAttempt++;
                // Wait 5 seconds before retrying
                await Task.Delay(5000);
            }
        }));
    }

    public bool testConnection(){
        return wsChannel.IsWebSocketConnected;
    }
}