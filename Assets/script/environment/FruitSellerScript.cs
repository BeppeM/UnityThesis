using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using WebSocketSharp;

public class FruitSellerScript : MASAbstract
{

    // Start is called before the first frame update
    void Start()
    {
        // Initialize new web socket connection
        initializeWebSocketConnection(OnMessage);
    }

    // Update is called once per frame
    void Update()
    {   
    }

    private void OnMessage(object sender, MessageEventArgs e) { }
}