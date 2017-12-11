using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodenDetector : MonoBehaviour {

    public bool isGrounded;
	// Use this for initialization

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
        Debug.Log("test");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isGrounded = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        isGrounded = true;
    }
}
