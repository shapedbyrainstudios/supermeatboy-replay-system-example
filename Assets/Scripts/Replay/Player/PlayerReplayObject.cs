using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReplayObject : ReplayObject
{
    private Animator animator;
    private SpriteRenderer sr;
    private ParticleSystem deathBurstParticles;

    private void Awake() 
    {
        animator = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        deathBurstParticles = GetComponentInChildren<ParticleSystem>();
        deathBurstParticles.Stop();
    }

    public override void SetDataForFrame(ReplayFrameInfo info) 
    {
        if (info.replayType != ReplayType.PLAYER) 
        {
            return;
        }
        PlayerReplayFrameInfo playerInfo = (PlayerReplayFrameInfo) info;
        // position
        this.transform.position = playerInfo.position;
        // animator
        animator.SetBool("isGrounded", playerInfo.isGrounded);
        animator.SetFloat("movementX", playerInfo.movement.x);
        animator.SetFloat("movementY", playerInfo.movement.y);
        // sprite alpha
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, playerInfo.spriteAlpha);
        sr.flipX = !playerInfo.facingRight;
        // particle burst
        if (playerInfo.deathThisFrame) 
        {
            deathBurstParticles.Play();
        }
    }
}
