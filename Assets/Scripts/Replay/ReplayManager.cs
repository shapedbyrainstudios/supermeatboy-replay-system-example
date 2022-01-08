using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayManager : MonoBehaviour
{
    private HashSet<Recorder> recorders;
    private bool isDoingReplay = false;

    public static ReplayManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Replay Manager in the scene.");
        }
        instance = this;
        // initialize the recorders list
        recorders = new HashSet<Recorder>();
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
        GameEventsManager.instance.onRestartLevel += OnRestartLevel;
    }

    public void RegisterRecorder(Recorder recorder) 
    {
        if (!recorders.Contains(recorder))
        {
            recorders.Add(recorder);
        }
        else 
        {
            Debug.Log("Tried to register recorder twice for gameobject: " + recorder.gameObject.name);
        }
    }

    private void Update() 
    {
        if (!isDoingReplay) 
        {
            return;
        }

        bool hasMoreFrames = false;

        // check to see if all of the recorders are done
        foreach (Recorder recorder in recorders) 
        {
            if (recorder.isDoingReplay) 
            {
                hasMoreFrames = true;
            }
        }

        // if we get here and hasMoreFrames is false, then the replay has finished and we need to restart
        if (!hasMoreFrames) 
        {
            RestartReplay();
        }
    }

    private void OnGoalReached() 
    {
        Debug.Log("Starting Replay");
        StartReplay();
    }

    private void OnRestartLevel() 
    {
        Debug.Log("Restarting Replay");
        EndReplay();
    }

    private void StartReplay() 
    {
        isDoingReplay = true;
        foreach (Recorder recorder in recorders) 
        {
            recorder.StartReplay();
        }
    }

    private void RestartReplay() 
    {
        foreach (Recorder recorder in recorders) 
        {
            recorder.RestartReplay();
        }
    }

    private void EndReplay() 
    {
        isDoingReplay = false;
        foreach (Recorder recorder in recorders) 
        {
            recorder.Reset();
        }
    }
}
