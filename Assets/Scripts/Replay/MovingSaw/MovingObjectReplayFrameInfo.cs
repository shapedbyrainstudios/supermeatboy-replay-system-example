using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectReplayInfo : ReplayFrameInfo
{
    public Vector3 scale { get; private set; }

    public MovingObjectReplayInfo(Vector3 position, Vector3 scale) 
    {
        // position
        this.position = position;
        // scale
        this.scale = scale;
        
    }
}
