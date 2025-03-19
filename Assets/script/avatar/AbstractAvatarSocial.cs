using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using WebSocketSharp;

public class AbstractAvatarSocial : AbstractAvatarWithEyesAndVoice
{
    protected MovementModel movementModel;

    protected override void Awake()
    {
        base.Awake();        
        // Set waypoints to follow
        movementModel = GetComponent<MovementModel>();
    }

    protected void resetStoppingDistance()
    {
        agent.stoppingDistance = 1.0f;
    }

    protected IEnumerator CheckIfReachedFriend(string friend)
    {
        while (true)
        {
            // Check if the agent has reached the destination
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    Debug.Log("Agent has reached his friend.");
                    transform.LookAt(GameObject.Find(friend).transform);
                    SendMessageToJaCaMoBrain(UnityJacamoIntegrationUtil
                        .createAndConvertJacamoMessageIntoJsonString("destinationReached", null,
                "reached_friend", null, friend));
                    yield break; // Exit the coroutine
                }
            }
            yield return new WaitForSeconds(0.1f); // Check every 0.1 seconds
        }
    }

    protected IEnumerator ActivateVisionCone()
    {
        yield return new WaitForSeconds(4.0f);
        EnableDisableVisionCone(true);
    }

    // Unity avatar receives message from jacamo agent
    protected override void OnMessage(object sender, MessageEventArgs e)
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
                        movementModel.IsStopped = false;
                        agent.ResetPath();
                        SetBaloonText("Walking");
                        movementModel.StartWalking();
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
                        movementModel.IsStopped = true;
                        agent.ResetPath();
                        EnableDisableVisionCone(false);
                        reachDestination(message.MessagePayload);
                    });
                    break;
                case "stopAgent":
                    print("Stopping the agent.");
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        SetBaloonText("I'm stopped");
                        movementModel.IsStopped = true;
                        agent.isStopped = true; ;
                        transform.LookAt(GameObject.Find(message.MessagePayload).transform);
                        EnableDisableVisionCone(false);
                    });
                    break;
                case "reachFriend":
                    // Avatar receives the type of artifact to reach
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        movementModel.IsStopped = true;
                        // Delete previous path and reach friend
                        agent.ResetPath();
                        agent.stoppingDistance = 8.0f;
                        EnableDisableVisionCone(false);
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

}
