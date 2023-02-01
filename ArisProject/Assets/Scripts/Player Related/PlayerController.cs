using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb2d;
    Vector2 moveInput;
    Animator anim;
    BoxCollider2D footCollider;
    CircleCollider2D gripBox;
    CapsuleCollider2D hitBox;

    [Header("Movement Related")]
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float climbSpeed = 2.25f;
    public float moveSpeed = 4f;
 
    float gravityScaleInit;
    bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        footCollider = GetComponent<BoxCollider2D>();
        gripBox = GetComponent<CircleCollider2D>();
        hitBox = GetComponent<CapsuleCollider2D>();
        gravityScaleInit = rb2d.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return; }

        Run();
        FlipSprite();
        Climb();
        Die();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive)  { return; }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        bool isGrounded = footCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if (!isAlive) { return; }

        if (!isGrounded) 
        {
            anim.SetBool("isGrounded", isGrounded);

            return;
        }
        else if (isGrounded)
        {
            anim.SetBool("isGrounded", isGrounded);
        }

        if (value.isPressed)
        {
            rb2d.velocity += new Vector2(rb2d.velocity.x, jumpForce);
            anim.SetTrigger("Jumping");
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
        if (!footCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            rb2d.gravityScale = gravityScaleInit;
            anim.SetBool("isClimbing", false);

            return;
        }
        else if (footCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            Vector2 climbVelocity = new Vector2(rb2d.velocity.x, moveInput.y * climbSpeed);
            rb2d.velocity = climbVelocity;
            rb2d.gravityScale = 0f;

            anim.SetBool("isClimbing", true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "WallGrip")
        {
            rb2d.gravityScale = 0.4f;
        }
        else
        {
            rb2d.gravityScale = gravityScaleInit;
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

    void Die()
    {
        if (hitBox.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            anim.SetTrigger("Dying");
        }
    }
}
