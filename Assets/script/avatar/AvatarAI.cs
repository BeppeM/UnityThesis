using Newtonsoft.Json;
using System;
using WebSocketSharp;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class AvatarAI : AbstractAvatar
{
    NavMeshAgent agent;
    private GameObject avatarBody;
    private GameObject avatarEyes;
    private TextMeshProUGUI baloonText;

    private void Awake()
    {
        agentFile = "shopper.asl";
        // Retrieve avatar parts
        avatarBody = transform.Find("Body").gameObject;
        avatarEyes = transform.Find("anchorVisionCone").gameObject;
        agent = GetComponent<NavMeshAgent>();

        if (Application.IsPlaying(gameObject))
        {
            initializeWebSocketConnection(OnMessage);
        }
        // Find the TextMeshPro component in the children of the avatar
        nameTextMeshPro  = transform.Find("avatarName").GetComponent<TextMeshPro>();
        nameTextMeshPro.text = name;

        // Initiating the baloon
        baloonText = gameObject.transform.Find("Canvas/BaloonBg/BaloonTxt").GetComponent<TextMeshProUGUI>();
        baloonText.text = "start";
    }

    // Unity avatar receives message from jacamo agent
    private void OnMessage(object sender, MessageEventArgs e)
    {
        string data = e.Data;
        print("Received message: " + data);
        WsMessage message = null;
        try
        {
            message = JsonConvert.DeserializeObject<WsMessage>(data);
            switch (message.MessageType)
            {
                case "wsInitialization":
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        print("Connection established for " + objInUse.name);
                    });
                    break;
                case "reachDestination":
                    print("Agent needs to reach destination.");
                    // Avatar receives the type of artifact to reach
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        SetBaloonText("New destination: " + message.MessagePayload);
                        reachDestination(message.MessagePayload);                        
                    });
                    break;
                case "conversation":
                    print("Agent is having a conversation.");                    
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        SetBaloonText(message.MessagePayload);                        
                    });
                    break;
                default:
                    print("Unknown message type for " + objInUse.name);
                    break;
            }
        }
        catch (Exception ex)
        {
            print("Error: " + ex.Message);
            print("Message could not be converted.");
            return;
        }
    }

    private void reachDestination(string dest)
    {
        agent.isStopped = false;
        agent.SetDestination(GameObject.Find(dest).transform.position);
    }

    public void SendMessageToJaCaMoBrain(string message)
    {
        wsChannel.sendMessage(message);
    }

    public void SetBaloonText(string message)
    {
        baloonText.text = message;
    }

    public void EnableDisableVisionCone(bool isActive)
    {
        avatarEyes.SetActive(isActive);
    }


}
