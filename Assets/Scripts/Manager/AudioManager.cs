using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioClip hitSound, attackSound, jumpSound, deathSound, walkSound;
    public AudioClip[] hitSounds;
    public AudioClip[] attackSounds;
    public AudioClip[] attackSounds2;
    public AudioClip[] jumpSounds;
    public AudioClip[] deathSounds;
    public AudioClip[] walkSounds;

    public AudioSource walkSource;

    public static AudioSource audioSource;

    private bool wait = true;

	// Use this for initialization
	void Start () { 
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
      //  Debug.Log(walkSource.isPlaying);
    }

    private void ChooseSound() {
        hitSound = hitSounds[Random.Range(0, hitSounds.Length)];
        attackSound = attackSounds2[Random.Range(0, attackSounds2.Length)];
        jumpSound = jumpSounds[Random.Range(0, jumpSounds.Length)];
        deathSound = deathSounds[Random.Range(0, deathSounds.Length)];
        walkSound = walkSounds[Random.Range(0, walkSounds.Length)];
    }

    public static void SetVolume(float volume) {
        audioSource.volume = volume;
    }

    public void PlaySound(string clip) {
        ChooseSound();
        audioSource.pitch = Random.Range(0.85f, 1.25f);
        switch(clip) {
            case "hit":
                audioSource.PlayOneShot(hitSound);
                walkSource.Pause();
                break;
            case "attack":
                audioSource.PlayOneShot(attackSound);
                audioSource.pitch = Random.Range(1.25f, 1.5f);

                walkSource.Pause();
                break;
            case "jump":
                audioSource.PlayOneShot(jumpSound);
                walkSource.Pause();
                break;
            case "death":
                audioSource.PlayOneShot(deathSound);

                audioSource.pitch = Random.Range(0.5f, 1f);

                walkSource.Pause();
                break;
            case "walk":
                //Debug.Log(wait);
                //StartCoroutine(Wait());
                //     InvokeRepeating("Walk", 1.0f, 1.5f);
                //    audioSource.PlayOneShot(walkSound);
       //         walkSource.Play();
                
                break;


        }
        //walkSource.Pause();
    }

    private void Walk() {
        audioSource.PlayOneShot(walkSound);
    }

    private IEnumerator Wait() {
        if (wait) {
            audioSource.PlayOneShot(walkSound);
            wait = false;
        }
        yield return new WaitForSeconds(2);
        wait = true;
    }
}
