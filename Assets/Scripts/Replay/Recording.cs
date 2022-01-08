using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recording
{
    public ReplayObject replayObject;
    public Queue<ReplayData> originalQueue { get; private set; }
    public Queue<ReplayData> replayQueue { get; private set; }

    public Recording(Queue<ReplayData> recordingQueue)
    {
        this.originalQueue = new Queue<ReplayData>(recordingQueue);
        this.replayQueue = new Queue<ReplayData>(recordingQueue);
    }

    public void RestartFromBeginning() 
    {
        this.replayQueue = new Queue<ReplayData>(originalQueue);
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
            ReplayData data = replayQueue.Dequeue();
            replayObject.SetDataForFrame(data);
            hasMoreFrames = true;
        }
        return hasMoreFrames;
    }

    public void InstantiateReplayObject(GameObject replayObjectPrefab) 
    {
        if (replayQueue.Count != 0) 
        {
            ReplayData startingData = replayQueue.Peek();
            ReplayObject replayObject = Object.Instantiate(replayObjectPrefab, startingData.position, Quaternion.identity)
                .GetComponent<ReplayObject>();
            this.replayObject = replayObject;
        }
    }

    public void DestroyReplayObjectIfExists() 
    {
        if (replayObject != null) 
        {
            Object.Destroy(replayObject.gameObject);
        }
    }

}
