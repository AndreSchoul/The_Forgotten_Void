using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespwanController : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Hero")
        {
            collision.GetComponent<PlayerController>().respawn();
        }
    }
}
