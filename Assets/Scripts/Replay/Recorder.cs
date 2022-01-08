using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    [Header("Type to replay for this recorder")]
    [SerializeField] private ReplayType replayType;

    [Header("Prefab to Instantiate")]
    [SerializeField] private GameObject replayObjectPrefab;
    
    public Queue<ReplayFrameInfo> replayQueue { get; private set; }

    public bool isDoingReplay { get; private set; } = false;

    private ReplayObject currentReplayObject;
    private Queue<ReplayFrameInfo> currentReplayQueue;

    private void Awake() 
    {
        replayQueue = new Queue<ReplayFrameInfo>();
    }

    private void Start() 
    {
        ReplayManager.instance.RegisterRecorder(this);
    }

    private void Update() 
    {
        if (!isDoingReplay) 
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
            // if we're finished replaying the queue, we're no longer replaying
            isDoingReplay = false;
        }
    }

    public void RecordReplayFrame(ReplayFrameInfo info) 
    {
        if (!isDoingReplay) 
        {
            replayQueue.Enqueue(info);
        }
    }

    public void StartReplay()
    {
        isDoingReplay = true;
        DestroyCurrentReplayObjectIfExists();
        currentReplayQueue = new Queue<ReplayFrameInfo>(replayQueue);
        InstantiateReplayObject();
        if (currentReplayObject.isNewCameraTarget) 
        {
            GameEventsManager.instance.ChangeCameraTarget(currentReplayObject.gameObject);
        }
    }

    public void Reset() 
    {
        isDoingReplay = false;
        this.replayQueue.Clear();
        DestroyCurrentReplayObjectIfExists();
    }

    private void InstantiateReplayObject() 
    {
        if (replayQueue.Count != 0) 
        {
            ReplayFrameInfo startingInfo = replayQueue.Peek();
            currentReplayObject = Instantiate(replayObjectPrefab, startingInfo.position, Quaternion.identity)
                .GetComponent<ReplayObject>();
            
            // TODO - do this better
            if (startingInfo.replayType == ReplayType.PLAYER)
            {
                currentReplayObject.isNewCameraTarget = true;
            }
        }
    }

    private void DestroyCurrentReplayObjectIfExists() 
    {
        if (currentReplayObject != null) 
        {
            Destroy(currentReplayObject.gameObject);
        }
    }
}
