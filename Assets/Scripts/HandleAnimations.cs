// Author: André Schoul

using UnityEngine;

public class HandleAnimations : MonoBehaviour {

    private Animator animator;
    //private JNRCharacterController characterController;
    private PlayerController characterController;

    void Awake() {
        animator = GetComponentInChildren<Animator>();
        //characterController = this.gameObject.GetComponent<JNRCharacterController>();
        characterController = this.gameObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update() {
        PlayAnimation();
    }
    
    private void HandleLayers() {
        if (!characterController.isGrounded) animator.SetLayerWeight(1, 1);
        else animator.SetLayerWeight(1, 0);
    }

    public void PlayAnimation() {
        HandleLayers();
        FlipSpriteRotation();
        switch (characterController.animationStance) {
            //case (JNRCharacterController.AnimationStance.idle):
            case (PlayerController.AnimationStance.idle):
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdling", true);
                animator.SetBool("isJumping", false);
                break;
            //case (JNRCharacterController.AnimationStance.walk):
            case (PlayerController.AnimationStance.walk):
                animator.SetBool("isWalking", true);
                animator.SetBool("isIdling", false);
                animator.SetBool("isJumping", false);
                animator.SetFloat("speed", characterController.movementSpeed);
                break;
            //case (JNRCharacterController.AnimationStance.jumpUp):
            case (PlayerController.AnimationStance.jumpUp):
                //if (Input.GetAxis("Horizontal") == 0) animator.SetFloat("movement", 0);
                //else animator.SetFloat("movement", 1);
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdling", false);
                animator.SetTrigger("isJumping");
                break;
            //case (JNRCharacterController.AnimationStance.jumpDown):
            case (PlayerController.AnimationStance.jumpDown):
                //if (Input.GetAxis("Horizontal") == 0) animator.SetFloat("movement", 0);
                //else animator.SetFloat("movement", 1);
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdling", false);
                animator.SetBool("isLanding", true);
                break;
        }
        if (characterController.isGrounded) animator.SetBool("isLanding", false);
    }

    private void FlipSpriteRotation() {
        if (Input.GetAxis("Horizontal") < 0) transform.rotation = Quaternion.Euler(0, 180, 0);
        if (Input.GetAxis("Horizontal") > 0) transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
