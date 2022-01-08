using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [Header("Speed")]
    public float speed = 3f;

    [Header("Waypoints")]
    public GameObject waypointsParent;

    private LinkedList<Transform> waypointsLinkedList;

    private LinkedListNode<Transform> targetNode;

    private SpriteRenderer sr;

    private Recorder recorder;

    private void Awake() 
    {
        InitializeWaypoints();
        targetNode = waypointsLinkedList.First;
        sr = GetComponent<SpriteRenderer>();
        recorder = GetComponent<Recorder>();
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
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
    }

    private void OnRestartLevel() 
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
    }

    private void Update() 
    {
        // calculate the move distance for this frame
        Vector3 newPosition = Vector3.MoveTowards(transform.position, targetNode.Value.position, speed * Time.deltaTime);
        float distanceFromWaypoint = (newPosition - targetNode.Value.position).magnitude;
        if (distanceFromWaypoint <= 0.1f) 
        {
            targetNode = FindNextNodeCircular();
        }
        // move
        this.transform.position = newPosition;
    }

    private void LateUpdate() 
    {
        ReplayFrameInfo info = new MovingObjectReplayInfo((Vector2) this.transform.position, (Vector2) this.transform.localScale);
        recorder.RecordReplayFrame(info);
    }

    private LinkedListNode<Transform> FindNextNodeCircular()
    {
        return targetNode.Next ?? targetNode.List.First;
    }

    private void InitializeWaypoints()
    {
        Transform[] possibleWaypoints = waypointsParent.gameObject.GetComponentsInChildren<Transform>();
        List<Transform> waypoints = new List<Transform>();
        foreach (Transform possibleWaypoint in possibleWaypoints)
        {
            // only add child objects tagged with "Waypoint"
            if (possibleWaypoint.gameObject.tag.Equals("Waypoint"))
            {
                waypoints.Add(possibleWaypoint);
            }
        }
        waypointsLinkedList = new LinkedList<Transform>(waypoints);
    }
}
