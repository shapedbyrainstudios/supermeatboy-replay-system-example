using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    [Header("Prefab to Instantiate")]
    [SerializeField] private GameObject replayObjectPrefab;

    [Header("Camera Targeting")]
    [SerializeField] private bool newCameraTarget = false;

    public Queue<ReplayData> recordingQueue { get; private set; }

    private List<Recording> recordings;
    private bool isDoingReplay = false;

    private void Awake() 
    {
        recordingQueue = new Queue<ReplayData>();
        recordings = new List<Recording>();
    }

    private void Start() 
    {
        // subscribe to events
        GameEventsManager.instance.onGoalReached += OnGoalReached;
        GameEventsManager.instance.onRestartLevel += OnRestartLevel;
    }

    private void OnDestroy() 
    {
        // unsubscribe from events
        GameEventsManager.instance.onGoalReached -= OnGoalReached;
        GameEventsManager.instance.onRestartLevel -= OnRestartLevel;
    }

    private void OnGoalReached() 
    {
        StartReplay();
    }

    private void OnRestartLevel() 
    {
        Reset();
    }

    private void Update() 
    {
        if (!isDoingReplay) 
        {
            return;
        }

        bool hasMoreFrames = false;
        foreach (Recording recording in recordings) 
        {
            // the result of this will always be from the
            // last recording in the list, which will be the successful playthrough
            hasMoreFrames = recording.PlayNextFrame();
        }

        // check if we're finished, so we can restart
        if (!hasMoreFrames) 
        {
            RestartReplay();
        }
    }

    public void RecordReplayFrame(ReplayData data) 
    {
        recordingQueue.Enqueue(data);
    }

    private void StartReplay() 
    {
        isDoingReplay = true;
        AddRecording();
        // instantiate all of the replay object in our scene
        foreach (Recording recording in recordings) 
        {
            recording.InstantiateReplayObject(replayObjectPrefab);
        }
        // change the camera target to the replay object
        if (newCameraTarget) 
        {
            Recording lastRecording = recordings[recordings.Count - 1];
            GameEventsManager.instance.ChangeCameraTarget(lastRecording.replayObject.gameObject);
        }
    }

    private void AddRecording() 
    {
        // add the recording
        recordings.Add(new Recording(recordingQueue));
        // reset the current recording queue for next time
        recordingQueue.Clear();
    }

    private void RestartReplay() 
    {
        isDoingReplay = true;
        // restart our queued data from the beginning
        foreach (Recording recording in recordings) 
        {
            recording.RestartFromBeginning();
        }
    }

    public void Reset() 
    {
        isDoingReplay = false;
        // reset the recorder to a clean slate
        recordingQueue.Clear();
        // cleanup replay objects
        foreach(Recording recording in recordings) 
        {
            recording.DestroyReplayObjectIfExists();
        }
        // re-initialize the recordings
        recordings = new List<Recording>();
    }

    public void StartNewRecording() 
    {
        AddRecording();
    }

}
