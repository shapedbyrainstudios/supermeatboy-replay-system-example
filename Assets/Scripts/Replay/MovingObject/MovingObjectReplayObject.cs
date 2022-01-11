using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectReplayObject : ReplayObject
{
    public override void SetDataForFrame(ReplayData data)
    {
        // typecast the data
        MovingObjectReplayData objData = (MovingObjectReplayData) data;
        // position
        this.transform.position = objData.position;
        // scale
        this.transform.localScale = objData.scale;
    }
}
