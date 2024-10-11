using UnityEngine;
using WebSocketSharp;

public class CounterScript : AbstractArtifact
{

    private int counter = 0;
    void Awake()
    {
        Type = ArtifactTypeEnum.Counter;
        // Initialize new web socket connection
        initializeWebSocketConnection(OnMessage);
    }

    // Update is called once per frame
    void Update() { }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("JacamoAgent"))
        {
            counter++;
            // Print the name of the object that entered the trigger
            Debug.Log("Trigger detected with " + other.gameObject.name);
            // Send message to JaCaMo
            wsChannel.sendMessage(UnityJacamoIntegrationUtil
                .createAndConvertJacamoMessageIntoJsonString(other.name, "signal_agent", "assign_number", null));
        }
    }

    private void OnMessage(object sender, MessageEventArgs e) { }
}
