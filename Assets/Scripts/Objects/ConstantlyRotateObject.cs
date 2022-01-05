using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantlyRotateObject : MonoBehaviour
{

    [Header("Configuration")]
    public bool clockwise = true;
    public float rotationSpeed = 100f;

    private void Update()
    {
        if (clockwise)
        {
            this.gameObject.transform.Rotate(0, 0, rotationSpeed * -1 * Time.deltaTime);
        }
        else
        {
            this.gameObject.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }
}
