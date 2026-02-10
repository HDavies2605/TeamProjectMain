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


    private Rigidbody2D rb;
    private Vector2 moveInput;

    //awake starts before everything (could use start if want)
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            moveInput = Vector2.zero;
            return;
        }

        moveInput = context.ReadValue<Vector2>().normalized;
    }
}
