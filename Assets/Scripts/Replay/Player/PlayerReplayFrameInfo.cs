using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReplayFrameInfo : ReplayFrameInfo
{
    public bool isGrounded { get; private set; }
    public Vector2 movement { get; private set; }
    public float spriteAlpha { get; private set; }
    public bool facingRight { get; private set; }
    public bool deathThisFrame { get; private set; }

    public PlayerReplayFrameInfo(Vector2 position, bool isGrounded, Vector2 movement, float spriteAlpha, bool facingRight, bool deathThisFrame) 
    {
        this.replayType = ReplayType.PLAYER;
        // position
        this.position = position;
        // animator info
        this.isGrounded = isGrounded;
        this.movement = movement;
        this.spriteAlpha = spriteAlpha;
        // facing dir
        this.facingRight = facingRight;
        // particle burst info
        this.deathThisFrame = deathThisFrame;
    }
}