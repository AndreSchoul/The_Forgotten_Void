using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpezialWalker : MonoBehaviour {

    public float speed = 3;
    private Animator ani;
    private bool inFight = false;
    public bool isLookingRight = true;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 targetPositionDelta;
    private Rigidbody2D rb;
    private RaycastHit2D hit;

    // Use this for initialization
    void Start()
    {
        ani = GetComponent<Animator>();
        ani.SetBool("isIdling", false);
        ani.SetBool("isWalking", true);
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        Vector2 rayStart;
        RaycastHit2D hit;
        RaycastHit2D bottomHit;


        if (isLookingRight)
        {
            rayStart = new Vector2(transform.position.x + 1f, transform.position.y + 0.5f);
            hit = Physics2D.Raycast(rayStart, new Vector2(1, 0), 1.5f);
            bottomHit = Physics2D.Raycast(rayStart, new Vector2(0, -1), 1.5f);
            Debug.DrawRay(rayStart, new Vector3(1, 0, 0), Color.red);
            Debug.DrawRay(rayStart, new Vector3(0, -1, 0), Color.green);

            if (bottomHit.transform == null)
            {
                Flip();
            }

            if (hit.transform != null && hit.transform.tag != "Hero")
            {
                Flip();
            }
            else
            {
                rb.velocity = new Vector2(speed, 0);

            }


        }
        else
        {
            rayStart = new Vector2(transform.position.x - 1f, transform.position.y + 0.5f);
            hit = Physics2D.Raycast(rayStart, new Vector2(-1, 0), 1.5f);
            bottomHit = Physics2D.Raycast(rayStart, new Vector2(0, -1), 1.5f);
            Debug.DrawRay(rayStart, new Vector3(-1, 0, 0), Color.red);
            Debug.DrawRay(rayStart, new Vector3(0, -1, 0), Color.green);

            if (bottomHit.transform == null)
            {
                Flip();
            }

            if (hit.transform != null && hit.transform.tag != "Hero")
            {
                Flip();
            }
            else
            {
                rb.velocity = new Vector2(speed * -1, 0);

            }

        }







    }

    public void Flip()
    {
        isLookingRight = !isLookingRight;
        Vector3 myScale = transform.localScale;
        myScale.x *= -1;
        transform.localScale = myScale;
    }
}
