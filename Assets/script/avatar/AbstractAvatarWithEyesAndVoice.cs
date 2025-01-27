using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using WebSocketSharp;

public abstract class AbstractAvatarWithEyesAndVoice : AbstractAvatar
{
    protected GameObject avatarBody;
    protected GameObject avatarEyes;
    protected TextMeshProUGUI baloonText;

    protected new void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();

        if (Application.IsPlaying(gameObject))
        {
            initializeAvatarWithEyes();
        }
    }

    protected void initializeAvatarWithEyes()
    {
        // Retrieve avatar parts
        avatarBody = transform.Find("Body").gameObject;
        avatarEyes = transform.Find("anchorVisionCone").gameObject;

        // Find the TextMeshPro component in the children of the avatar
        nameTextMeshPro = transform.Find("avatarName").GetComponent<TextMeshPro>();
        nameTextMeshPro.text = name;

        // Initiating the baloon
        baloonText = gameObject.transform.Find("Canvas/BaloonBg/BaloonTxt").GetComponent<TextMeshProUGUI>();
        baloonText.text = "start";
    }

    public void SetBaloonText(string message)
    {
        baloonText.text = message;
    }

    public void EnableDisableVisionCone(bool isActive)
    {
        avatarEyes.SetActive(isActive);
    }

    public void SendMessageToJaCaMoBrain(string message)
    {
        wsChannel.sendMessage(message);
    }

    // Unity avatar receives message from jacamo agent
    protected void OnMessage(object sender, MessageEventArgs e)
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
}
