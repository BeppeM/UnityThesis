using Newtonsoft.Json;
using System;
using WebSocketSharp;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AvatarAI : AbstractAvatar
{
    NavMeshAgent agent;
    private GameObject avatarBody;
    private GameObject avatarEyes;
    private TextMeshProUGUI baloonText;
    private AutonomousWalking autonomousWalking;

    private void Awake()
    {
        agentFile = "shopper.asl";
        // Retrieve avatar parts
        avatarBody = transform.Find("Body").gameObject;
        avatarEyes = transform.Find("anchorVisionCone").gameObject;
        agent = GetComponent<NavMeshAgent>();
        autonomousWalking = GetComponent<AutonomousWalking>();

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
                case "startWalking":
                    // Avatar receives the type of artifact to reach
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        resetStoppingDistance();
                        autonomousWalking.IsStopped = false;
                        agent.ResetPath();
                        SetBaloonText("Walking");                                               
                        autonomousWalking.StartWalking();
                        StartCoroutine(ActivateVisionCone());
                    });
                    break;
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
                case "stopAgent":
                    print("Stopping the agent.");                    
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        SetBaloonText("I'm stopped");
                        autonomousWalking.IsStopped = true;
                        agent.isStopped = true;                        ;
                        transform.LookAt(GameObject.Find(message.MessagePayload).transform);
                        EnableDisableVisionCone(false);
                    });
                    break;
                case "reachFriend":
                    // Avatar receives the type of artifact to reach
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        EnableDisableVisionCone(false);
                        // Delete previous path and reach friend
                        agent.ResetPath();
                        agent.stoppingDistance = 8.0f;
                        autonomousWalking.IsStopped = true;
                        reachDestination(message.MessagePayload);                        
                        StartCoroutine(CheckIfReachedFriend(message.MessagePayload));
                    });
                    break;
                case "conversation":
                    // Avatar receives the type of artifact to reach
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

    private void resetStoppingDistance()
    {
        agent.stoppingDistance = 1.0f;
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


    IEnumerator CheckIfReachedFriend(string friend)
    {
        while (true)
        {
            // Check if the agent has reached the destination
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    Debug.Log("Agent has reached his friend.");
                    SendMessageToJaCaMoBrain(UnityJacamoIntegrationUtil
                        .createAndConvertJacamoMessageIntoJsonString("destinationReached", null,
                "reached_friend", null, friend));
                    yield break; // Exit the coroutine
                }
            }
            yield return new WaitForSeconds(0.1f); // Check every 0.1 seconds
        }
    }

    IEnumerator ActivateVisionCone()
    {
        yield return new WaitForSeconds(3.0f);
        EnableDisableVisionCone(true);
    }




}
