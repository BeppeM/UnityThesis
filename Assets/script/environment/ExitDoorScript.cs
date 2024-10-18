using Unity.VisualScripting;
using UnityEngine;

public class ExitDoorScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        print("CIAOAOAOAOAOAOA");
        if (other.gameObject.CompareTag("JacamoAgent"))
        {
            Destroy(other);
        }
    }

    private void OnMessage(object sender, WebSocketSharp.MessageEventArgs e) { }    
}
