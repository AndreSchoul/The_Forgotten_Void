// Author: André Schoul

using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

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
    [HideInInspector]
    public float    movementSpeed;
    public float minMovementSpeed  = 15f;
    public float maxMovementSpeed  = 25f;
    public float jumpVelocity      = 21f;
    public float fallMultiplier    = 15f;
    public float lowJumpMultiplier =  5f;
    public List<Transform> spawnPointsHeros = new List<Transform>();
    public List<Transform> spawnPointsEnemies = new List<Transform>();
    public static AudioSource audio_JnR;
    public static bool isInFight = false;
    public static GameObject hero;

    private float originOffset = 1.41f;
    private float walkTime = 0f;
    private float distanceToObject;
    private Transform groundDetector;
    private LayerMask whatIsGrounded;
    private Rigidbody2D rb2d;

    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.freezeRotation = true;
        groundDetector = this.gameObject.transform.Find("GroundDetector");
        whatIsGrounded |= (1 << LayerMask.NameToLayer("Ground"));
        hero = this.gameObject;
        movementSpeed = minMovementSpeed;
        audio_JnR = this.GetComponent<AudioSource>();
        audio_JnR.Play();
    }

    private void Update() {
        Movement();
        Jump();
    }

    private void FixedUpdate() {
        Vector2 direction = new Vector2(1, 0);
        if (rb2d.transform.forward.z < 0) direction *= -1;
        RaycastHit2D hit = CheckRaycast(direction);
        if (hit.collider) {
            if (rb2d.transform.forward.z < 0) distanceToObject = transform.position.x - originOffset - hit.collider.transform.position.x - 2.6f;
            else distanceToObject = transform.position.x + originOffset - hit.collider.transform.position.x;
        }
    }

    private void Jump() {
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
        
    private void Movement() {
        movementSpeed = Mathf.Clamp(movementSpeed, minMovementSpeed, maxMovementSpeed);
        if (distanceToObject < 0.35f && distanceToObject > -0.35f) {
            if (rb2d.transform.forward.z > 0 && Input.GetAxis("Horizontal") > 0 || rb2d.transform.forward.z < 0 && Input.GetAxis("Horizontal") < 0) {                
                walkTime = 0f;
                movementSpeed = minMovementSpeed;
                animationStance = AnimationStance.idle;
                //return;
            }
        }
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
        }
    }

    private RaycastHit2D CheckRaycast(Vector2 direction) {
        float directionOriginOffset = originOffset * (direction.x > 0 ? 1 : -1);
        Vector2 startingPosition = new Vector2(transform.position.x + directionOriginOffset, transform.position.y);
        return Physics2D.Raycast(startingPosition, direction);
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
            if (other.tag == "Item") {

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


