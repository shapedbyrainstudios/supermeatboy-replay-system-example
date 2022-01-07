using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    [Header("Prefab to Instantiate")]
    [SerializeField] private GameObject replayObjectPrefab;
    private ReplayObject currentReplayObject;

    private Queue<ReplayFrameInfo> replayQueue;
    private Queue<ReplayFrameInfo> currentReplayQueue;

    private bool doingReplay = false;

    private void Start() 
    {
        replayQueue = new Queue<ReplayFrameInfo>();
        // subscribe to events
        GameEventsManager.instance.onRecordReplayFrame += OnRecordReplayFrame;
        GameEventsManager.instance.onGoalReached += OnGoalReached;
        GameEventsManager.instance.onRestartLevel += OnRestartLevel;
    }

    private void OnDestroy() 
    {
        // unsubscribe from events
        GameEventsManager.instance.onRecordReplayFrame -= OnRecordReplayFrame;
        GameEventsManager.instance.onGoalReached -= OnGoalReached;
        GameEventsManager.instance.onRestartLevel += OnRestartLevel;
    }

    private void OnRecordReplayFrame(ReplayFrameInfo info) 
    {
        if (!doingReplay) 
        {
            replayQueue.Enqueue(info);
        }

        Debug.Log("QUEUE COUNT: " + replayQueue.Count);
    }

    private void OnGoalReached() 
    {
        // start replay when the goal has been reached
        StartReplay();
        Debug.Log("Starting Replay");
    }

    private void StartReplay() 
    {
        InitializeReplayObjects();
        GameEventsManager.instance.ReplayStarted(currentReplayObject);
        doingReplay = true;
    }

    private void RestartReplay() 
    {
        CleanupReplayObject();
        StartReplay();
        Debug.Log("Restarting Replay");
    }

    private void InitializeReplayObjects() 
    {
        currentReplayQueue = new Queue<ReplayFrameInfo>(replayQueue);
        if (currentReplayQueue.Count != 0) 
        {
            ReplayFrameInfo startingInfo = currentReplayQueue.Peek();
            currentReplayObject = Instantiate(replayObjectPrefab, startingInfo.position, Quaternion.identity)
                .GetComponent<ReplayObject>();
        }
    }

    private void OnRestartLevel() 
    {
        doingReplay = false;
        CleanupReplayObject();
        replayQueue.Clear();
    }

    private void CleanupReplayObject() 
    {
        if (currentReplayObject != null) 
        {
            Destroy(currentReplayObject.gameObject);
        }
    }

    private void Update() 
    {
        if (!doingReplay) 
        {
            return;
        }

        if (currentReplayQueue.Count != 0) 
        {
            ReplayFrameInfo info = currentReplayQueue.Dequeue();
            currentReplayObject.SetDataForFrame(info);
        }
        else 
        {
            RestartReplay();
        }
    }

    
}
