using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneController : MonoBehaviour {
    private Vector3 spawnPosition;
    private float toleranzY;
    public GameObject particle;
    private AudioSource dieSound;
    public bool firstObject;
	// Use this for initialization


    private void Start()
    {
        spawnPosition = transform.position;
        toleranzY = spawnPosition.y - 5;
        dieSound = GetComponent<AudioSource>();
        if (firstObject == false)
        {
            dieSound.Play();
        }
        
    }

    // Update is called once per frame
    void Update () {        
        if (transform.position.y < toleranzY)
        {
            
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            firstObject = false;
            Instantiate(gameObject, spawnPosition, Quaternion.identity);          
            Destroy(gameObject);            
        }
	}

    private void OnDestroy()
    {
        
        Destroy(Instantiate(particle, transform.position, Quaternion.identity), 2);       
    }
}
