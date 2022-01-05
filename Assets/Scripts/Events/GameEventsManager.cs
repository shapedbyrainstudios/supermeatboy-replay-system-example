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
}
