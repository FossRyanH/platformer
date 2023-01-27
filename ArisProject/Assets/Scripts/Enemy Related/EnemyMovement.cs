using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D rigidbody;
    RaycastHit2D hitDetection;
    Animator anim;

    [SerializeField] float distanceDetection = 0.2f;
    public float moveSpeed = 1.75f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        hitDetection = Physics2D.Raycast(transform.position, Vector2.right, distanceDetection);

        if (hitDetection.collider != null)
        {
            if (hitDetection.collider.tag == "Ground")
            {
                FlipSprite();
            }
        }

        Debug.DrawRay(transform.position, Vector2.right * distanceDetection, Color.red);
    }

    void Move()
    {
        Vector2 enemyVelocity = new Vector2(moveSpeed, rigidbody.velocity.y);
        rigidbody.velocity = enemyVelocity; 

        bool isMoving = Mathf.Abs(rigidbody.velocity.x) > Mathf.Epsilon;
        anim.SetBool("isMoving", isMoving);
    }

    void FlipSprite()
    {
        bool hasHorizontalSpeed = Mathf.Abs(rigidbody.velocity.x) > Mathf.Epsilon;
        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rigidbody.velocity.x), 1f);
        }
    }
}
