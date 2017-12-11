using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePopUp : MonoBehaviour {
    public Animator animator;

    private Text damageText;

    void Awake() {      
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        damageText = GetComponentInChildren<Text>();
        Destroy(gameObject, clipInfo[0].clip.length);
    }
    	
    public void SetText(string text) {
        damageText.text = text;
    }
}
