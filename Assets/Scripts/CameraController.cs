using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private GameObject playerPosition;

    private void Start()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Hero");     
    }
    // Update is called once per frame
    void FixedUpdate () {
        if (!JNRCharacterController.isInFight && !BlendenController.isBlenden) {
            Vector3 newPosition = new Vector3(playerPosition.transform.position.x, playerPosition.transform.position.y, -20);
            transform.position = newPosition;
        }
    }
}
