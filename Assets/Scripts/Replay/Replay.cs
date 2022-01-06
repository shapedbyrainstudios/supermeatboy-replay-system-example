using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Replay : MonoBehaviour
{
    private Queue<ReplayFrameInfo> replayQueue;
    private Queue<ReplayFrameInfo> currentReplayQueue;

    [SerializeField] private GameObject replayObjectPrefab;
    private ReplayObject replayObject;

    private bool replayStarted = false;

    private void Start() 
    {
        replayQueue = new Queue<ReplayFrameInfo>();
        // subscribe to events
        GameEventsManager.instance.onCaptureReplayFrame += OnCaptureReplayFrame;
        GameEventsManager.instance.onResetReplay += OnResetReplay;
        GameEventsManager.instance.onGoalReached += OnGoalReached;
    }

    private void OnDestroy() 
    {
        // unsubscribe from events
        GameEventsManager.instance.onCaptureReplayFrame -= OnCaptureReplayFrame;
        GameEventsManager.instance.onResetReplay -= OnResetReplay;
        GameEventsManager.instance.onGoalReached -= OnGoalReached;
    }

    private void OnCaptureReplayFrame(ReplayFrameInfo info) 
    {
        if (!replayStarted) 
        {
            replayQueue.Enqueue(info);
        }
    }

    private void OnResetReplay()
    {
        replayStarted = false;
        if (replayObject != null) 
        {
            Destroy(replayObject);
        }
        replayQueue.Clear();
    }

    private void OnGoalReached() 
    {
        currentReplayQueue = new Queue<ReplayFrameInfo>(replayQueue);
        if (currentReplayQueue.Count != 0) 
        {
            ReplayFrameInfo startingInfo = currentReplayQueue.Peek();
            replayObject = Instantiate(replayObjectPrefab, startingInfo.position, Quaternion.identity)
                .GetComponent<ReplayObject>();
        }
        replayStarted = true;
    }

    private void RestartReplay() 
    {
        if (replayObject != null) 
        {
            Destroy(replayObject.gameObject);
        }
        OnGoalReached();
    }

    private void Update() 
    {
        if (!replayStarted) 
        {
            return;
        }

        if (currentReplayQueue.Count != 0) 
        {
            ReplayFrameInfo info = currentReplayQueue.Dequeue();
            replayObject.SetDataForFrame(info);
        }
        else 
        {
            Debug.Log("REACHED END OF REPLAY. RESTARTING.");
            RestartReplay();
        }
    }

    
}
