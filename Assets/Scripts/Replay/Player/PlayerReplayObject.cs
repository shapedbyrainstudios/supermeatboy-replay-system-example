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

    public override void SetDataForFrame(ReplayData data) 
    {
        // typecast the info
        PlayerReplayData playerData = (PlayerReplayData) data;
        // position
        this.transform.position = playerData.position;
        // animator
        animator.SetBool("isGrounded", playerData.isGrounded);
        animator.SetFloat("movementX", playerData.movement.x);
        animator.SetFloat("movementY", playerData.movement.y);
        // sprite alpha
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, playerData.spriteAlpha);
        sr.flipX = !playerData.facingRight;
        // particle burst
        if (playerData.deathThisFrame) 
        {
            deathBurstParticles.Play();
        }
    }
}
