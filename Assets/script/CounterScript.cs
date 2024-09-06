using UnityEngine;
using WebSocketSharp;

public class CounterScript : MASAbstract
{
    // Start is called before the first frame update
    void Start()
    {
        // Initialize new web socket connection
        initializeWebSocketConnection(OnMessage);
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
            sendMessageToJaCaMo("increment", other.gameObject.name);
        }
    }

    private void sendMessageToJaCaMo(string actionToPerform, string agentName)
    {
        WsMessage wsMessage = PrepareMessageUtil.prepareMessage(objInUse.name, actionToPerform, agentName);
        string jsonString = JsonUtility.ToJson(wsMessage);
        Debug.Log(wsMessage.getActionToPerform());
        Debug.Log(jsonString);
        webSocketChannel.sendMessage(jsonString);
    }

    private void OnMessage(object sender, MessageEventArgs e) { }
}
