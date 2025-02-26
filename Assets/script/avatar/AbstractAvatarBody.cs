using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class AbstractAvatarBody : MonoBehaviour
{
    protected GameObject root;
    protected AbstractAvatar mainAvatarScript;
    protected string artifactReached;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        artifactReached = "";
    }

    protected virtual void OnTriggerEnter(Collider other) { }
    protected virtual void OnTriggerExit(Collider other) { }
}