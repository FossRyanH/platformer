using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb2d;
    Vector2 moveInput;
    Animator anim;
    CapsuleCollider2D collider;

    [SerializeField] float jumpForce = 10f;
    [SerializeField] float climbSpeed = 4.5f;

    public float moveSpeed = 15f;

    float gravityScaleInit;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        collider = GetComponent<CapsuleCollider2D>();
        gravityScaleInit = rb2d.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        FlipSprite();
        Climb();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        /* This is checking if the player is touching the ground. If the player is touching the ground, it will
        allow the player to jump. */;
        if (!collider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

        if (value.isPressed)
        {
            rb2d.velocity += new Vector2(rb2d.velocity.x, jumpForce);
        }
    }

    void Run()
    {
        /* This is the code that is making the player move. */
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, rb2d.velocity.y);
        rb2d.velocity = playerVelocity;

        bool isMoving = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        anim.SetBool("isRunning", isMoving);
    }

    void Climb()
    {
        if (!collider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            rb2d.gravityScale = gravityScaleInit;
            anim.SetBool("isClimbing", false);

            return;
        }
        else if (collider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            Vector2 climbVelocity = new Vector2(rb2d.velocity.x, moveInput.y * climbSpeed);
            rb2d.velocity = climbVelocity;
            rb2d.gravityScale = 0f;

            anim.SetBool("isClimbing", true);
        }
    }

    void FlipSprite()
    {  
        /* Checking if the player is moving horizontally. If the player is moving horizontally, it will flip
        the sprite. */
        bool hasHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;

        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb2d.velocity.x), 1f);
        }
    }
}
