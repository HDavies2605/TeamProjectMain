using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movementSpeed = 5f;

    [Header("Acceleration Settings")]
    [SerializeField] private bool useAcceleration = true;

    [SerializeField, Range(0f, 200f)]
    private float acceleration = 25f;

    [SerializeField, Range(0f, 200f)]
    private float deceleration = 35f;


    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool playingFootsteps = false;
    public float footstepSpeed = 0.5f;

    //awake starts before everything (could use start if want)
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();  //get animator from child object (PlayerSprite)
    }

    void FixedUpdate()
    {
        Vector2 targetVelocity = moveInput * movementSpeed;

        //no acceleration mode
        if (!useAcceleration)
        {
            // snappy / instant movement
            rb.linearVelocity = targetVelocity;
            return;
        }

        //choose whether accelerating or decelerating
        float accelRate = moveInput.sqrMagnitude > 0
            ? acceleration //pressing input -> speed up
            : deceleration; // no input pressed -> slow down

        rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, targetVelocity, accelRate * Time.fixedDeltaTime);

        //StartFootstep
        if(rb.linearVelocity.magnitude > 0 && !playingFootsteps)
        {
            StartFootsteps();
        }
        else if(rb.linearVelocity.magnitude == 0)
        {
            StopFootsteps();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        animator.SetBool("isWalking", true);

        if (context.canceled)
        {
            //if input is released, stop moving and set last input for idle animation
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);

            moveInput = Vector2.zero;
            return;
        }

        moveInput = context.ReadValue<Vector2>().normalized;
        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);
    }

    void StartFootsteps()
    {
        playingFootsteps = true;
        InvokeRepeating(nameof(PlayFootstep), 0f, footstepSpeed);
    }

    void StopFootsteps()
    {
        playingFootsteps = false;
        CancelInvoke(nameof(PlayFootstep));
    }

    void PlayFootstep()
    {
        SoundEffectManager.Play("Footstep");
    }
    
}
