// Author: André Schoul

using System.Collections.Generic;
using UnityEngine;

public class JNRCharacterController : MonoBehaviour {

    public enum AnimationStance {
        idle,
        walk,
        jumpUp,
        jumpDown,
        attack
    }
    [HideInInspector]
    public AnimationStance animationStance;
    [HideInInspector]
    public bool isGrounded = true;
    public List<Transform> spawnPointsHeros   = new List<Transform>();
    public List<Transform> spawnPointsEnemies = new List<Transform>();
    public float fallMultiplier    = 15f;
    public float lowJumpMultiplier =  2f;
    public float minMovementSpeed  = 20f;
    public float maxMovementSpeed  = 30f;
    public float jumpVelocity      = 20f;
    public float movementSpeed;
    public static GameObject hero;
    public static AudioSource audio_JnR;
    public static bool isInFight = false;

    private float walkTime = 0f;
    private Animator animator;
    private Transform groundDetector;
    private LayerMask whatIsGrounded;
    private Rigidbody2D rb2d;

    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.freezeRotation = true;
        animator = GetComponentInChildren<Animator>();
        groundDetector = this.gameObject.transform.Find("GroundDetector");
        whatIsGrounded |= (1 << LayerMask.NameToLayer("Ground"));
        movementSpeed = minMovementSpeed;
        hero = this.gameObject;
        audio_JnR = this.GetComponent<AudioSource>();
        audio_JnR.Play();
    }

    private void FixedUpdate() { 
        Movement();
        Jump();
    }

    private void Jump() {
        if (!isInFight) {
            isGrounded = Physics2D.OverlapCircle(groundDetector.position, 1F, whatIsGrounded);
            if (isGrounded && Input.GetButtonDown("Jump")) {
                rb2d.velocity = Vector2.up * jumpVelocity;
                animationStance = AnimationStance.jumpUp;
            }
            if (rb2d.velocity.y > 0 && !Input.GetButton("Jump")) {
                rb2d.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
                animationStance = AnimationStance.jumpUp;
            }
            if (rb2d.velocity.y < 0) {
                rb2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
                animationStance = AnimationStance.jumpDown;
            }
        }
    }

    private void Movement() {
        if (!isInFight) {
            movementSpeed = Mathf.Clamp(movementSpeed, minMovementSpeed, maxMovementSpeed);
            if (Input.GetAxis("Horizontal") == 0) {
                walkTime = 0f;
                movementSpeed = minMovementSpeed;
                animationStance = AnimationStance.idle;
            } else {
                transform.Translate(Vector3.right * Time.deltaTime * movementSpeed);
                walkTime += Time.deltaTime;
                if (walkTime > 1f) movementSpeed += 0.085f;
                else movementSpeed = minMovementSpeed;
                animationStance = AnimationStance.walk;
                FlipSpriteRotation();
            }
        }
    }

    public void FlipSpriteRotation() {
        if (Input.GetAxis("Horizontal") < 0) transform.rotation = Quaternion.Euler(0, 180, 0);
        if (Input.GetAxis("Horizontal") > 0) transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    
    /// <summary>
    /// Checks collider to know what the hero is interacting with and acts accordingly.
    /// </summary>
    /// <param name = "other">Collided object</param> 
    void OnTriggerEnter2D(Collider2D other) {
        if (!isInFight) {
            // for map transition   
            /*if (other.tag == "Teleporter") {
                GameManager.instance.nextSpawnPoint = col.spawnPointName;
                GameManager.instance.sceneToLoad = col.sceneToLoad;
            }*/
            // for interaction with items
            if(other.tag == "Item") {

            }
            // for battle
            if (other.tag == "Enemy") {
                //isInFight = true;
                GameObject.Find("Blende").GetComponent<BlendenController>().blenden();
                isInFight = true;
                GameManager.instance.gui.SetActive(true);
                BattleStateMachine bsm = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
                CharacterAmountType enemy = other.gameObject.GetComponent<CharacterAmountType>();
                CharacterAmountType hero = this.gameObject.GetComponent<CharacterAmountType>();
                bsm.SpawnCharacters(enemy.characters.Count, enemy.characters, spawnPointsEnemies, false);
                bsm.SpawnCharacters(hero.characters.Count, hero.characters, spawnPointsHeros, true);   
                this.gameObject.SetActive(false);
                other.gameObject.SetActive(false);
                GameManager.instance.collidedEnemy = other.gameObject;
                GameManager.PlayMusic(bsm.GetComponent<AudioSource>());
            }
        }
    }
}
