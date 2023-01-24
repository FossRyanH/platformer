using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb2d;
    Vector2 moveInput;
    Animator anim;

    public float moveSpeed = 15f;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        FlipSprite();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, rb2d.velocity.y);
        rb2d.velocity = playerVelocity;

        bool isMoving = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        anim.SetBool("isRunning", isMoving);
    }

    void FlipSprite()
    {
        /* Checking if the player is moving horizontally and flipping the sprite. */
        bool hasHorizontalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        transform.localScale = new Vector2(Mathf.Sign(rb2d.velocity.x), 1f);
    }
}