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
    public event Action onRestartLevel;
    public event Action<ReplayFrameInfo> onCaptureReplayFrame;
    public event Action onResetReplay;

    public void GoalReached() 
    {
        if (onGoalReached != null) 
        {
            onGoalReached();
        }
    }
    
    public void RestartLevel() 
    {
        if (onRestartLevel != null) 
        {
            onRestartLevel();
        }
    }

    public void CaptureReplayFrame(ReplayFrameInfo info) 
    {
        if (onCaptureReplayFrame != null) 
        {
            onCaptureReplayFrame(info);
        }
    }

    public void ResetReplay() 
    {
        if (onResetReplay != null) 
        {
            onResetReplay();
        }
    }
}
