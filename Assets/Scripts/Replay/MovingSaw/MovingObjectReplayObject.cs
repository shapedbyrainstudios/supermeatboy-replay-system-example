using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectReplayObject : ReplayObject
{
    public override void SetDataForFrame(ReplayFrameInfo info) 
    {
        if (info.replayType != ReplayType.MOVING_OBJECT) 
        {
            return;
        }
        MovingObjectReplayInfo objInfo = (MovingObjectReplayInfo) info;
        // position
        this.transform.position = objInfo.position;
        // scale
        this.transform.localScale = objInfo.scale;
    }
}
