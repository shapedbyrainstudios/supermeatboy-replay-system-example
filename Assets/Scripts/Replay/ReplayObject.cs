using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ReplayObject : MonoBehaviour
{
    public abstract void SetDataForFrame(ReplayData data);
}
