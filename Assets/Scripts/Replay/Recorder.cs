using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    [Header("Prefab to Instantiate")]
    [SerializeField] private GameObject replayObjectPrefab;

    [Header("Camera Targeting")]
    [SerializeField] private bool newCameraTarget = false;
    
    public Queue<ReplayFrameInfo> recordingQueue { get; private set; }

    public List<Recording> recordings;

    public bool isDoingReplay { get; private set; } = false;

    private void Awake() 
    {
        recordingQueue = new Queue<ReplayFrameInfo>();
        recordings = new List<Recording>();
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

        bool hasMoreFrames = false;
        foreach(Recording recording in recordings) 
        {
            // the result of this will always be whether the last
            // recording in the list has finished.
            hasMoreFrames = recording.PlayNextFrame();
        }

        // check if we're finished
        if (!hasMoreFrames) 
        {
            // if we're finished replaying the queue, we're no longer replaying
            isDoingReplay = false;
        }
    }

    public void RecordReplayFrame(ReplayFrameInfo info) 
    {
        recordingQueue.Enqueue(info);
    }

    public void StartReplay()
    {
        // add the current recording to our recordings list
        AddRecording();
        // instantiate all of the replay objects
        foreach (Recording recording in recordings) 
        {
            recording.InstantiateReplayObject(replayObjectPrefab);
        }
        // if this recorder is intended to produce the new camera target, send out
        //  an event using the instantiated object for the last recording that was added
        if (newCameraTarget) 
        {
            Recording lastRecording = recordings[recordings.Count - 1];
            GameEventsManager.instance.ChangeCameraTarget(lastRecording.replayObject.gameObject);
        }
        isDoingReplay = true;
    }

    public void RestartReplay() 
    {
        // instantiate all of the replay objects
        foreach (Recording recording in recordings) 
        {
            recording.RestartFromBeginning();
        }
        // start doing replay again
        isDoingReplay = true;
    }

    private void AddRecording() 
    {
        // add the current recording to a the list of recordings
        Queue<ReplayFrameInfo> archivedRecordingQueue = new Queue<ReplayFrameInfo>(recordingQueue);
        recordings.Add(new Recording(archivedRecordingQueue));
        // reset the current recording queue for the next recording
        this.recordingQueue.Clear();
    }

    public void Reset() 
    {
        isDoingReplay = false;
        this.recordingQueue.Clear();
        CleanupReplayObjects();
        this.recordings = new List<Recording>();
    }

    private void CleanupReplayObjects() 
    {
        foreach (Recording recording in recordings) 
        {
            recording.DestroyReplayObjectIfExists();
        } 
    }

    public void StartNewRecording() 
    {
        AddRecording();
    }
}
