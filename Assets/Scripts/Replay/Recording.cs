using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recording
{
    public ReplayObject replayObject;
    public Queue<ReplayFrameInfo> recordingQueue { get; private set; }
    public Queue<ReplayFrameInfo> replayQueue { get; private set; }

    public Recording(Queue<ReplayFrameInfo> recordingQueue)
    {
        this.recordingQueue = recordingQueue;
        this.replayQueue = new Queue<ReplayFrameInfo>(recordingQueue);
    }

    public void RestartFromBeginning() 
    {
        this.replayQueue = new Queue<ReplayFrameInfo>(recordingQueue);
    }

    public void DestroyReplayObjectIfExists() 
    {
        if (replayObject != null) 
        {
            Object.Destroy(replayObject.gameObject);
        }
    }

    public void InstantiateReplayObject(GameObject replayObjectPrefab) 
    {
        if (replayQueue.Count != 0) 
        {
            ReplayFrameInfo startingInfo = replayQueue.Peek();
            ReplayObject replayObject = Object.Instantiate(replayObjectPrefab, startingInfo.position, Quaternion.identity)
                .GetComponent<ReplayObject>();
            this.replayObject = replayObject;
        }
    }

    public bool PlayNextFrame() 
    {
        if (replayObject == null) 
        {
            Debug.LogWarning("Tried to play next frame of recording, but replayObject was null.");
        }
        
        bool hasMoreFrames = false;
        if (replayQueue.Count != 0) 
        {
            ReplayFrameInfo info = replayQueue.Dequeue();
            replayObject.SetDataForFrame(info);
            hasMoreFrames = true;
        }
        return hasMoreFrames;
    }
}
