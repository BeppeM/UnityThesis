using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class AbstractAvatarBody : MonoBehaviour
{
    protected GameObject root;
    protected AbstractAvatarWithEyesAndVoice mainAvatarScript;

    // Start is called before the first frame update
    void Awake()
    {
        root = transform.parent.gameObject;
        mainAvatarScript = root.GetComponent<AbstractAvatarWithEyesAndVoice>();
    }

    protected virtual void OnTriggerEnter(Collider other) {}

    protected virtual void OnTriggerExit(Collider other) {}
}