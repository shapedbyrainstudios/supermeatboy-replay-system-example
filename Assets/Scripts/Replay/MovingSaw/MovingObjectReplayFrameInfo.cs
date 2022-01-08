using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectReplayInfo : ReplayFrameInfo
{
    public Vector2 scale { get; private set; }

    public MovingObjectReplayInfo(Vector2 position, Vector2 scale) 
    {
        this.replayType = ReplayType.MOVING_OBJECT;
        // position
        this.position = position;
        // scale
        this.scale = scale;
        
    }
}
