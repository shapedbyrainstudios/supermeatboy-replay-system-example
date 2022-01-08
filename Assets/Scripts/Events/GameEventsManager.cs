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

    public event Action<GameObject> onChangeCameraTarget;
    public void ChangeCameraTarget(GameObject newTarget) 
    {
        if (onChangeCameraTarget != null) 
        {
            onChangeCameraTarget(newTarget);
        }
    }
}
