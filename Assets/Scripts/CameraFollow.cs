using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject target;
    public float lerpTime = 0.6f;

    private Vector3 targetPos;
    private float interpVelocity;

    // Use this for initialization
    void Start() {
        targetPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (target && !JNRCharacterController.isInFight) {
            Vector3 posNoZ = transform.position;
            posNoZ.z = target.transform.position.z;
            Vector3 targetDirection = (target.transform.position - posNoZ);
            interpVelocity = targetDirection.magnitude * 5f;
            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, targetPos, lerpTime);
        }
    }
}
