using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ReplayObject : MonoBehaviour
{
    public bool isNewCameraTarget = false;
    public abstract void SetDataForFrame(ReplayFrameInfo info);
}
