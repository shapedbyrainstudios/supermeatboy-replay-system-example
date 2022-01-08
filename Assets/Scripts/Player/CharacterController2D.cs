using System.Collections;
using UnityEngine;

// This script is a basic 2D character controller that allows
// the player to run and jump. It uses Unity's new input system,
// which needs to be set up accordingly for directional movement
// and jumping buttons.

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{

    [Header("Movement Params")]
    [SerializeField] private float runSpeed = 6.0f;
    [SerializeField] private float jumpSpeed = 8.0f;
    [SerializeField] private float gravityScale = 20.0f;

    [Header("Respawn Point")]
    [SerializeField] private Transform respawnPoint;

    [Header("Particles")]
    [SerializeField] private ParticleSystem deathBurstParticles;

    // components attached to player
    private BoxCollider2D coll;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;
    private Recorder recorder;

    // input parameters for movement
    Vector2 moveDirection = Vector2.zero;
    bool jumpPressed = false;

    // other
    private bool facingRight = true;
    private bool isGrounded = false;
    private bool disableMovement = false;
    private bool deathThisFrame = false;

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        recorder = GetComponent<Recorder>();

        rb.gravityScale = gravityScale;
        deathBurstParticles.Stop();
    }

    private void Start() 
    {
        // subscribe to events
        GameEventsManager.instance.onGoalReached += OnGoalReached;
        GameEventsManager.instance.onRestartLevel += OnRestartLevel;
    }

    private void OnDestroy() 
    {
        // unsubscribe from events
        GameEventsManager.instance.onGoalReached -= OnGoalReached;
        GameEventsManager.instance.onRestartLevel -= OnRestartLevel;
    }

    private void LateUpdate()
    {
        // record frame info for replay
        ReplayFrameInfo info = new PlayerReplayFrameInfo(this.transform.position, isGrounded, rb.velocity, sr.color.a, facingRight, deathThisFrame);
        recorder.RecordReplayFrame(info);
        deathThisFrame = false;
    }

    private void FixedUpdate()
    {
        if (disableMovement) 
        {
            return;
        }

        HandleInput();

        UpdateIsGrounded();

        HandleHorizontalMovement();

        HandleJumping();

        UpdateFacingDirection();

        UpdateAnimator();
    }

    private void HandleInput() 
    {
        moveDirection = InputManager.instance.GetMoveDirection();
        jumpPressed = InputManager.instance.GetJumpPressed();
    }

    private void UpdateIsGrounded()
    {
        Bounds colliderBounds = coll.bounds;
        float colliderRadius = coll.size.x * 0.4f * Mathf.Abs(transform.localScale.x);
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, colliderRadius * 0.9f, 0);
        // Check if player is grounded
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPos, colliderRadius);
        // Check if any of the overlapping colliders are not player collider, if so, set isGrounded to true
        this.isGrounded = false;
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != coll)
                {
                    this.isGrounded = true;
                    break;
                }
            }
        }
    }

    private void HandleHorizontalMovement()
    {
        rb.velocity = new Vector2(moveDirection.x * runSpeed, rb.velocity.y);
    }

    private void HandleJumping()
    {
        if (isGrounded && jumpPressed)
        {
            isGrounded = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }

    private void UpdateFacingDirection()
    {
        // set facing direction
        if (moveDirection.x > 0.1f)
        {
            facingRight = true;
        }
        else if (moveDirection.x < -0.1f)
        {
            facingRight = false;
        }

        // rotate according to direction
        // we do this instead of using the 'flipX' spriteRenderer option because our player is made up of multiple sprites
        if (facingRight)
        {
            this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, 0, this.transform.eulerAngles.z);
        }
        else
        {
            this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, 180, this.transform.eulerAngles.z);
        }
    }

    private void UpdateAnimator()
    {
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("movementX", rb.velocity.x);
        animator.SetFloat("movementY", rb.velocity.y);
    }

    private IEnumerator HandleDeath() 
    {
        // freeze player movemet
        rb.gravityScale = 0;
        disableMovement = true;
        rb.velocity = Vector3.zero;
        // prevent other collisions
        coll.enabled = false;
        // hide the player visual
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
        // play the death particles
        deathBurstParticles.Play();
        // keep track of death for the replay
        deathThisFrame = true;
        
        yield return new WaitForSeconds(0.4f);
        
        Respawn();
        // start a new recording for the replay
        recorder.StartNewRecording();
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        // if we collided with anything in the harmful layer, death occurs
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Harmful")))
        {
            StartCoroutine(HandleDeath());
        }
    }

    private void OnGoalReached() 
    {
        // freeze movement
        rb.gravityScale = 0;
        rb.velocity = Vector3.zero;
        disableMovement = true;
        // hide player visual
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
    }

    private void OnRestartLevel() 
    {
        // ensure we don't jump right away on reset, since
        // the submit button is the same as the jump button.
        // this is specific to the InputManager in this project
        InputManager.instance.RegisterJumpPressedThisFrame(); 
        // respawn
        Respawn();
    }

    private void Respawn() 
    {
        // enable movement
        rb.gravityScale = gravityScale;
        coll.enabled = true;
        disableMovement = false;
        // show player visual
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
        // move the player to the respawn point
        this.transform.position = respawnPoint.position;
        // send out event
        GameEventsManager.instance.PlayerRespawn();
    }

}