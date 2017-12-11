using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneController : MonoBehaviour {
    private bool isGrounded = true;
    private Rigidbody2D rb2d;
    private float minX, maxX;
    public GameObject detectorLeft;
    public GameObject detectorRight;
    
	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();        
	}

    void Update()
    {
        BodenDetector leftGrounded = detectorLeft.GetComponent<BodenDetector>();
        BodenDetector rightGrounded = detectorLeft.GetComponent<BodenDetector>();

        if (!leftGrounded.isGrounded == false)
        {            
            Debug.Log("KeinLinks");
            minX = detectorLeft.transform.position.x;
            rb2d.velocity = new Vector3(0, 0);
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        }
        if (!rightGrounded == false)
        {
            Debug.Log("KeinRechts");
            maxX = detectorRight.transform.position.x;
            rb2d.velocity = new Vector3(0, 0);
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        }
    }
    // Update is called once per frame
}
