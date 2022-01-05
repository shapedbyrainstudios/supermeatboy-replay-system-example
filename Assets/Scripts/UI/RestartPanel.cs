using UnityEngine;

public class RestartPanel : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject panel;

    private void Start() 
    {
        panel.SetActive(false);
        GameEventsManager.instance.onGoalReached += OnGoalReached;
        GameEventsManager.instance.onRestartLevel += OnRestartLevel;
    }

    private void OnDestroy() 
    {
        GameEventsManager.instance.onGoalReached += OnGoalReached;
        GameEventsManager.instance.onRestartLevel += OnRestartLevel;
    }

    private void OnGoalReached() 
    {
        panel.SetActive(true);
    }

    public void OnRestartLevel() 
    {
        panel.SetActive(false);
    }

}
