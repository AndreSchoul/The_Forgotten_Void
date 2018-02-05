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

    private float originOffset = 1.1f;
    private float walkTime = 0f;
    private float distanceToObject;
    private float distanceObjectToWall;
    private Transform groundDetector;
    private LayerMask whatIsGrounded;
    private Rigidbody2D rb2d;
    private RaycastHit2D playerHit;
    private RaycastHit2D objectHit;
    private Vector2 spawnPositionJNR;

    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.freezeRotation = true;
        groundDetector = this.gameObject.transform.Find("GroundDetector");
        whatIsGrounded |= (1 << LayerMask.NameToLayer("Ground"));
        hero = this.gameObject;
        movementSpeed = minMovementSpeed;
        audio_JnR = this.GetComponent<AudioSource>();
        audio_JnR.Play();
        spawnPositionJNR = new Vector2(transform.position.x, transform.position.y);
    }

    private void Update() {
        Movement();
        Jump();
    }

    private void FixedUpdate() {
        Vector2 direction = new Vector2(1, 0);
        float objectOffset = 2.6f;
        if (rb2d.transform.forward.z < 0) {
            direction *= -1;
            objectOffset = 0.1f;
        } 
        playerHit = CheckRaycast(this.gameObject, direction, originOffset, false);
        if (playerHit.collider != null) {
            if (playerHit.collider.gameObject.layer == 9) objectHit = CheckRaycast(playerHit.collider.gameObject, direction, objectOffset, true);
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
        if (playerHit.collider != null) {
            if (playerHit.collider.gameObject.layer == 9 && playerHit.distance < 0.1f && objectHit.distance < 0.1f || playerHit.collider.gameObject.layer != 9 && playerHit.distance < 0.1f) {
                if (rb2d.transform.forward.z > 0 && Input.GetAxis("Horizontal") > 0 || rb2d.transform.forward.z < 0 && Input.GetAxis("Horizontal") < 0) {
                    walkTime = 0f;
                    movementSpeed = minMovementSpeed;
                    animationStance = AnimationStance.idle;
                    return;
                }
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

    private RaycastHit2D CheckRaycast(GameObject from, Vector2 direction, float offset, bool withLayerMask) {
        float directionOriginOffset = offset * (direction.x > 0 ? 1 : -1);
        Vector2 startingPosition = new Vector2(from.transform.position.x + directionOriginOffset, from.transform.position.y);
        if (withLayerMask) {
            int layerMask = 1 << 8;
            return Physics2D.Raycast(startingPosition, direction, Mathf.Infinity, layerMask);
        } else return Physics2D.Raycast(startingPosition, direction);
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

    public void respawn()
    {
        transform.position = spawnPositionJNR;
    }

    public void setRespawnPosition(Vector3 spawnPoint)
    {
        spawnPositionJNR = spawnPoint;
    }
}


