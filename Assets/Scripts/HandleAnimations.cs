// Author: André Schoul

using UnityEngine;

public class HandleAnimations : MonoBehaviour {

    private Animator animator;
    private JNRCharacterController characterController;

    void Awake() {
        animator = GetComponentInChildren<Animator>();
        characterController = this.gameObject.GetComponent<JNRCharacterController>();
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
        switch (characterController.animationStance) {
            case (JNRCharacterController.AnimationStance.idle):
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdling", true);
                animator.SetBool("isJumping", false);
                break;
            case (JNRCharacterController.AnimationStance.walk):
                animator.SetBool("isWalking", true);
                animator.SetBool("isIdling", false);
                animator.SetBool("isJumping", false);
                animator.SetFloat("speed", characterController.movementSpeed);
                break;
            case (JNRCharacterController.AnimationStance.jump):
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdling", false);
                animator.SetTrigger("isJumping");
                break;
            case (JNRCharacterController.AnimationStance.land):
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdling", false);
                animator.SetBool("isLanding", true);
                break;
        }
        if (characterController.isGrounded) animator.SetBool("isLanding", false);
    }
}
