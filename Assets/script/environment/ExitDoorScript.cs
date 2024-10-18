using UnityEngine;

public class ExitDoorScript
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("JacamoAgent"))
        {                        
            other.GetComponent<ReachDestination>().stopWalking();
        }
    }

    private void OnMessage(object sender, WebSocketSharp.MessageEventArgs e) { }    
}
