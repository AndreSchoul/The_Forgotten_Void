// Author: André Schoul

using UnityEngine;

public class CheckCameraFrustum : MonoBehaviour {

    private float backgroundWidth;
    private Transform hero;

    private void Awake() {
        backgroundWidth = transform.GetComponent<Renderer>().bounds.size.x - 0.2f;
        hero = GameObject.FindGameObjectWithTag("Hero").transform;
    }

    private void Update() {
        if (hero.position.x - transform.position.x >  backgroundWidth && hero.position.x > transform.position.x) transform.position += new Vector3(backgroundWidth * 2, 0, 0);
        if (hero.position.x - transform.position.x < -backgroundWidth && hero.position.x < transform.position.x) transform.position -= new Vector3(backgroundWidth * 2, 0, 0);
    }
}

    