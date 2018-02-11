using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespwanController : MonoBehaviour {

    private float audioVolume;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Hero") {
            collision.gameObject.SetActive(false);
            StartCoroutine(Wait(collision.gameObject));
            GameObject.Find("Blende").GetComponent<BlendenController>().blenden();
            //collision.GetComponent<PlayerController>().Respawn();
            AudioManager audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            audioVolume = AudioManager.audioSource.volume;
            AudioManager.SetVolume(1);
            audioManager.PlaySound("death");
        }
    }

    IEnumerator Wait(GameObject player) {
        yield return new WaitForSeconds(1f);
        player.GetComponent<PlayerController>().Respawn();
        player.SetActive(true);
        GameManager.PlayMusic(PlayerController.audio_JnR);
        AudioManager.SetVolume(0.5f);
    }
}
