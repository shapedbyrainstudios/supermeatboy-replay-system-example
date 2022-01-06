using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayObject : MonoBehaviour
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

    public void SetDataForFrame(ReplayFrameInfo info) 
    {
        // position
        this.transform.position = info.position;
        // animator
        animator.SetBool("isGrounded", info.isGrounded);
        animator.SetFloat("movementX", info.movement.x);
        animator.SetFloat("movementY", info.movement.y);
        // sprite alpha
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, info.spriteAlpha);
        // particle burst
        if (info.deathThisFrame) 
        {
            deathBurstParticles.Play();
        }
    }
}
