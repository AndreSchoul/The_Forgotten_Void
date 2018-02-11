using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            GameManager gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
            gm.LoadNextScene("DesertLevel");
        }
    }
}