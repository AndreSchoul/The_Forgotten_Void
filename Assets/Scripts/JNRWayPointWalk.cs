using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JNRWayPointWalk : MonoBehaviour {

    public float movementSpeed = 1.05f;
    public List<Vector3> waypointPositions;
    int currentWaypoint = 0;
    private Animator ani;
    private bool inFight = false;
    private bool isLookingRight = true;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 targetPositionDelta;

	// Use this for initialization
	void Start () {
        ani = GetComponent<Animator>();
        ani.SetBool("isIdling", false);
        ani.SetBool("isWalking", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!JNRCharacterController.isInFight)
        {
            WayPointWalk();
            Move();
            ani.SetBool("isIdling", false);
            ani.SetBool("isWalking", true);
        }
        else
        {
            ani.SetBool("isIdling", true);
            ani.SetBool("isWalking", false);
        }
        
    }

    void WayPointWalk()
    {
        Vector3 targetPosition = waypointPositions[currentWaypoint];
        targetPositionDelta = targetPosition - transform.position;

        if(targetPositionDelta.sqrMagnitude <= 1)
        {
            currentWaypoint++;
            if(currentWaypoint >= waypointPositions.Count)
            {
                currentWaypoint = 0;
            }
        }
        else
        {
            if(targetPositionDelta.x > 0)
            {
                isLookingRight = true;
                if(transform.localScale.x > 0)
                {
                    Flip();
                }
            }
            else
            {
                isLookingRight = false;
                if(transform.localScale.x < 0)
                {
                    Flip();
                }
            }
        }
    }

    void Move()
    {
        moveDirection = targetPositionDelta.normalized * movementSpeed;
        transform.Translate(moveDirection * Time.deltaTime, Space.World);
    }

    public void Flip()
    {
        isLookingRight = !isLookingRight;
        Vector3 myScale = transform.localScale;
        myScale.x *= -1;
        transform.localScale = myScale;
    }
}
