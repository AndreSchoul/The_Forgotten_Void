using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour {

    private bool isChecked = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isChecked && collision.tag == "Hero")
        {            
            isChecked = true;
            collision.GetComponent<PlayerController>().SetRespawnPosition(transform.position);
            while(transform.rotation.x > 0.7)
            {
                transform.Rotate(-0.1f, 0, 0);
            }
            
        }
    }
}
