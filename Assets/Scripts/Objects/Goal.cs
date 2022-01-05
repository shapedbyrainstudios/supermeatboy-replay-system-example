using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider) 
    {
        GameEventsManager.instance.GoalReached();
    }
}
