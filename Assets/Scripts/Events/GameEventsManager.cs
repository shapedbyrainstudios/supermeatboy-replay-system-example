using System;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");
        }
        instance = this;
    }

    public event Action onGoalReached;
    public void GoalReached() 
    {
        if (onGoalReached != null) 
        {
            onGoalReached();
        }
    }
    
    public event Action onRestartLevel;
    public void RestartLevel() 
    {
        if (onRestartLevel != null) 
        {
            onRestartLevel();
        }
    }

    public event Action<ReplayFrameInfo> onRecordReplayFrame;
    public void RecordReplayFrame(ReplayFrameInfo info) 
    {
        if (onRecordReplayFrame != null) 
        {
            onRecordReplayFrame(info);
        }
    }

    public event Action<ReplayObject> onReplayStarted;
    public void ReplayStarted(ReplayObject replayObject) 
    {
        if (onReplayStarted != null) 
        {
            onReplayStarted(replayObject);
        }
    }
}
