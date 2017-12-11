using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JnR_PlayerController : MonoBehaviour {

    public float    fallMultiplier = 15f;
    public float lowJumpMultiplier =  2f;
    public float movementSpeed     = 15f;
    public float minMovementSpeed  = 20f;
    public float maxMovementSpeed  = 30f;
    public float jumpVelocity      = 20f;

    private float walkTime = 0f;
    private Animator animator;
    private bool isGrounded = true;
    private Transform groundDetector;
    private LayerMask whatIsGrounded;
    private Rigidbody2D rb2d;
    private AnimationStance animationStance;
    private enum AnimationStance {
        idle,
        walk,
        jump,
        land,
        attack
    }

    private void Awake () {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        groundDetector = this.gameObject.transform.Find("GroundDetector");
        whatIsGrounded |= (1 << LayerMask.NameToLayer("Ground"));
    }

    private void FixedUpdate() {
        PlayAnimation();
        Movement();      
    }

    private void Update() {
        Jump();
    }

    private void Jump() {
        isGrounded = Physics2D.OverlapCircle(groundDetector.position, 1F, whatIsGrounded);
        if (isGrounded && Input.GetButtonDown("Jump")) {
            rb2d.velocity = Vector2.up * jumpVelocity;
            animationStance = AnimationStance.jump;
        }
        if (rb2d.velocity.y > 0 && !Input.GetButton("Jump")) {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            animationStance = AnimationStance.jump;
        }
        if (rb2d.velocity.y < 0) {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            animationStance = AnimationStance.land;
        }
    }

    private void Movement() {
        movementSpeed = Mathf.Clamp(movementSpeed, minMovementSpeed, maxMovementSpeed);
        if (Input.GetAxis("Horizontal") == 0) {
            walkTime = 0f;
            movementSpeed = minMovementSpeed;
            animationStance = AnimationStance.idle;
        } else {
            transform.Translate(Vector3.right * Time.deltaTime * movementSpeed);
            walkTime += Time.deltaTime;
            if (walkTime > 1f) movementSpeed += 0.08f;
            else movementSpeed = minMovementSpeed;
            animationStance = AnimationStance.walk;
            FlipSpriteRotation();
        }
    }

    private void HandleLayers() {
        if (!isGrounded) animator.SetLayerWeight(1, 1);
        else animator.SetLayerWeight(1, 0);
    }

    public void PlayAnimation() {
        HandleLayers();
        switch (animationStance) {
            case (AnimationStance.idle):
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdling" , true);
                animator.SetBool("isJumping", false);
                break;
            case (AnimationStance.walk):
                animator.SetBool("isWalking", true);
                animator.SetBool("isIdling" , false);
                animator.SetBool("isJumping", false);
                break;
            case (AnimationStance.jump):             
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdling" , false);
                animator.SetTrigger("isJumping");
                break;
            case (AnimationStance.land):
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdling", false);
                animator.SetBool("isLanding", true);
                break;
        }
        if(isGrounded) animator.SetBool("isLanding", false);
    }

    public void FlipSpriteRotation() {
        if (Input.GetAxis("Horizontal") < 0) transform.rotation = Quaternion.Euler(0, 180, 0);
        if (Input.GetAxis("Horizontal") > 0) transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}

