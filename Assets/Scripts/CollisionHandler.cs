using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionHandler : MonoBehaviour {
    /*
    public List<Transform> spawnPointsHeros = new List<Transform>();
    public List<Transform> spawnPointsEnemies = new List<Transform>();
    JNRCharacterController playerController;

    void Awake() {
        //playerController = this.gameObject.GetComponentInChildren<JNRCharacterController>();
        //spawnPointsHeros = playerController.spawnPointsHeros;
        //spawnPointsEnemies = playerController.spawnPointsEnemies;
    }

    
    /// <summary>
    /// Checks collider to know what the hero is interacting with and acts accordingly.
    /// </summary>
    /// <param name = "player">Collided object</param> 
    void OnTriggerEnter2D(Collider2D player) {
        if (!JNRCharacterController.isInFight) {
            CollisionHandler col = player.gameObject.GetComponent<CollisionHandler>();
            if (player.tag == "Hero") {
                GameObject.Find("Blende").GetComponent<BlendenController>().blenden();
                JNRCharacterController.isInFight = true;
                GameManager.instance.gui.SetActive(true);
                BattleStateMachine bsm = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
                CharacterAmountType enemy = player.gameObject.GetComponent<CharacterAmountType>();
                CharacterAmountType hero = this.gameObject.GetComponent<CharacterAmountType>();
                bsm.SpawnCharacters(enemy.characters.Count, enemy.characters, GameObject.FindGameObjectWithTag("Hero").GetComponentInChildren<JNRCharacterController>().spawnPointsEnemies, false);
                bsm.SpawnCharacters(hero.characters.Count, hero.characters, GameObject.FindGameObjectWithTag("Hero").GetComponentInChildren<JNRCharacterController>().spawnPointsHeros, true);
                this.gameObject.SetActive(false);
                player.gameObject.SetActive(false);
                GameManager.instance.collidedEnemy = player.gameObject;
                GameManager.PlayMusic(bsm.GetComponent<AudioSource>());
            }
        }
    }*/
}
