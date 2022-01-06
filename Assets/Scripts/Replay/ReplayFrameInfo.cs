using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayFrameInfo
{
    public Vector2 position { get; private set; }
    public bool isGrounded { get; private set; }
    public Vector2 movement { get; private set; }
    public float spriteAlpha { get; private set; }
    public bool deathThisFrame { get; private set; }

    public ReplayFrameInfo(Vector2 position, bool isGrounded, Vector2 movement, float spriteAlpha, bool deathThisFrame) 
    {
        // position
        this.position = position;
        // animator info
        this.isGrounded = isGrounded;
        this.movement = movement;
        this.spriteAlpha = spriteAlpha;
        // particle burst info
        this.deathThisFrame = deathThisFrame;
    }
}
