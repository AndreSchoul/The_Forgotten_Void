// Author: André Schoul

using System.Collections.Generic;
using UnityEngine;

public class JNRCharacterController : MonoBehaviour {

    public Animator animator;
    public float speed     =  10f;
    public float jumpForce = 800f;
    public List<Transform> spawnPointsHeros   = new List<Transform>();
    public List<Transform> frontPointsHeros   = new List<Transform>();
    public List<Transform> spawnPointsEnemies = new List<Transform>();
    public List<Transform> frontPointsEnemies = new List<Transform>();
    public static bool isInFight = false;
    public bool isJump           = false;
    public bool isLookingRight   = true;
    public bool isAction         = true;
    public static GameObject hero;
    public static AudioSource audio_JnR;
    public Transform groundCheck;
    public LayerMask whatIsGrounded;

    private bool isGrounded = true;
    private Rigidbody2D rb2d;

    // Use this for initialization
    void Start() {
        /*if(GameManager.instance.nextSpawnPoint != "") {
            GameObject spawnPoint = GameObject.Find(GameManager.instance.nextSpawnPoint);
            transform.position = spawnPoint.transform.position;
            GameManager.instance.nextSpawnPoint = "";
        } else if(GameManager.instance.lastHeroPosition != Vector3.zero) {
            transform.position = GameManager.instance.lastHeroPosition;
            GameManager.instance.lastHeroPosition = Vector3.zero;
        }*/
        rb2d = GetComponent<Rigidbody2D>();
        hero = this.gameObject;
        if (!isInFight) {
            transform.position = GameManager.instance.nextHeroPosition;
        }
        audio_JnR = this.GetComponent<AudioSource>();
        audio_JnR.Play();
    }

    // Update is called once per frame
    void Update () {
        if (!isInFight) {
            if (Input.GetAxis("Horizontal") != 0) {
                animator.SetBool("isWalking", true);
                animator.SetBool("isIdling", false);
                transform.Translate(Vector3.right * Time.deltaTime * speed);
                if (Input.GetAxis("Horizontal") < 0) {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                if (Input.GetAxis("Horizontal") > 0) {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            } else {
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdling", true);
            }
            if (Input.GetButtonDown("Jump") && isGrounded) {
                isJump = true;
            }
            if (Input.GetButtonDown("Fire1")) {
                isAction = true;
            } else {
                isAction = false;
            }
        }
	}

    void FixedUpdate() {
        // TODO: Animation
        float h = Input.GetAxisRaw("Horizontal");
        rb2d.velocity = new Vector2(h * speed, rb2d.velocity.y);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 1F, whatIsGrounded);
        if (isJump) {
            rb2d.AddForce(new Vector2(0, jumpForce));
            isJump = false;
        }
    }

    /// <summary>
    /// Checks collider to know what the hero is interacting with and acts accordingly.
    /// </summary>
    /// <param name = "other">Collided object</param> 
    void OnTriggerEnter2D(Collider2D other) {
        if (!isInFight) {
            CollisionHandler col = other.gameObject.GetComponent<CollisionHandler>();
            // for map transition
            if (other.tag == "Teleporter") {
                GameManager.instance.nextSpawnPoint = col.spawnPointName;
                GameManager.instance.sceneToLoad = col.sceneToLoad;
            }
            // for interaction with items
            if(other.tag == "Item") {

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
}
