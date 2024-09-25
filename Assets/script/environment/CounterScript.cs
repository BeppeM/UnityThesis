using UnityEngine;
using WebSocketSharp;

public class CounterScript : AbstractArtifact
{
    void Awake()
    {
        Type = AgentArtifactTypeEnum.counter;
        // Initialize new web socket connection
        initializeWebSocketConnection(OnMessage);
    }

    // Update is called once per frame
    void Update() { }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("JacamoAgent"))
        {
            // Print the name of the object that entered the trigger
            Debug.Log("Trigger detected with " + other.gameObject.name);
            // Send message to JaCaMo
            wsMessage = UnityJacamoIntegrationUtil.prepareMessage(null, "increment", other.gameObject.name, null);
            UnityJacamoIntegrationUtil.sendMessageToJaCaMo(wsMessage, wsChannel);
        }
    }

    private void OnMessage(object sender, MessageEventArgs e) { }
}
