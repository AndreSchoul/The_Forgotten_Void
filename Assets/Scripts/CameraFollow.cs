// Author: André Schoul

using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public float lerpTime = 0.6f;

    private GameObject target;
    private Vector3 targetPos;
    private float interpVelocity;

    // Use this for initialization
    void Start() {
        target = GameObject.FindGameObjectWithTag("Hero");
        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update() {
        //if (target && !JNRCharacterController.isInFight) {
        if (target && !PlayerController.isInFight) {
            Vector3 posNoZ = transform.position;
            posNoZ.z = target.transform.position.z;
            Vector3 targetDirection = (target.transform.position - posNoZ);
            interpVelocity = targetDirection.magnitude * 5f;
            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, targetPos, lerpTime);
        }
    }
}
