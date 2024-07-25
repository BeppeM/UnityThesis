using System;
using System.Collections.Concurrent;
using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{
    private static MainThreadDispatcher _instance;
    private static ConcurrentQueue<Action> _actionQueue = new ConcurrentQueue<Action>();

    public static MainThreadDispatcher Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("MainThreadDispatcher");
                _instance = go.AddComponent<MainThreadDispatcher>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    public static void Enqueue(Action action)
    {
        _actionQueue.Enqueue(action);
    }

    private void Update()
    {
        while (_actionQueue.TryDequeue(out Action action))
        {
            action();
        }
    }
}

