using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<Action> _executionQueue = new Queue<Action>();

    public static UnityMainThreadDispatcher Instance()
    {
        if (!_instance)
        {
            throw new Exception("UnityMainThreadDispatcher instance not found. Ensure you have added the UnityMainThreadDispatcher script to the scene.");
        }
        return _instance;
    }

    private static UnityMainThreadDispatcher _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        lock (_executionQueue)
        {
            while (_executionQueue.Count > 0)
            {
                _executionQueue.Dequeue().Invoke();
            }
        }
    }

    public void Enqueue(Action action)
    {
        lock (_executionQueue)
        {
            _executionQueue.Enqueue(action);
        }
    }

    public void Enqueue(IEnumerator action)
    {
        Enqueue(() => StartCoroutine(action));
    }
}
