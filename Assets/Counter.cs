using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class Counter : MonoBehaviour
{


    private WebSocketChannel webSocketChannel;
    public GameObject envObj;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize new web socket connection
        WSConnectionInfoModel wSConnectionInfoModel = new WSConnectionInfoModel("ws://localhost:8885", "OBJECT", envObj.name);
        webSocketChannel = new WebSocketChannel(wSConnectionInfoModel, OnMessage);
    }

    // Update is called once per frame
    void Update() { }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Print the name of the object that entered the trigger
            Debug.Log("Trigger detected with " + other.gameObject.name);
            // Send message to JaCaMo
            webSocketChannel.sendMessage("increment");
        }
    }

    private void OnMessage(object sender, MessageEventArgs e) { }
}
