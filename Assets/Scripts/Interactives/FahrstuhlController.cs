using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FahrstuhlController : MonoBehaviour {

    public float speed = 1;
    public int top = 20;
    public int down = 20;
    private bool onTheWayToTop = true;
    private float startPosition;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 targetPositionDelta;


    private void Start()
    {
        startPosition = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {        
            WayPointWalk();
            Move();
    }

    void WayPointWalk()
    {
        if (onTheWayToTop)
        {
            if(transform.position.y < startPosition + top - 1)
            {
                Vector3 targetPosition = new Vector3(transform.position.x, startPosition + top, 0);
                targetPositionDelta = targetPosition - transform.position;
                transform.Translate(targetPositionDelta * Time.deltaTime, Space.World);
            }
            else
            {
                onTheWayToTop = false;
            }

        }
        else
        {
            if (transform.position.y > startPosition - down + 1)
            {
                Vector3 targetPosition = new Vector3(transform.position.x, startPosition - down, 0);
                targetPositionDelta = targetPosition - transform.position;
                transform.Translate(targetPositionDelta * Time.deltaTime, Space.World);
            }
            else
            {
                onTheWayToTop = true;
            }
        }
    }

    void Move()
    {
        moveDirection = targetPositionDelta.normalized * speed;        
    }
}
