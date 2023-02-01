using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    CapsuleCollider2D groundDetection;
    CircleCollider2D hitBox;

    public float moveSpeed = 1.75f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector2 enemyVelocity = new Vector2(moveSpeed, rb.velocity.y);
        rb.velocity = enemyVelocity; 

        bool isMoving = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        anim.SetBool("isMoving", isMoving);
    }

    void FlipSprite()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(rb.velocity.x)), 1f);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Ground")
        {
            moveSpeed = -moveSpeed;
            FlipSprite();
        }
    }
}
