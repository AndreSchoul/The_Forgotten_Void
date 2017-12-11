// Author: André Schoul

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public GameObject heroCharacter;
    public GameObject gui;
    public GameObject collidedEnemy;
    public Vector3 nextHeroPosition;
    public Vector3 lastHeroPosition; 
    public string sceneToLoad;
    public string lastScene;
    public int enemyAmount;
    public string nextSpawnPoint;
    public List<GameObject> enemiesToBattle = new List<GameObject>();

    private static AudioSource currentAudio;

	void Awake () {
        // check if an instance exists
		if(instance == null) { 
            instance = this;
        }
        // if another instance exists 
        else if (instance != this) {
            Destroy(gameObject);
        }
        // persistent through different scenes
     /*   DontDestroyOnLoad(gameObject);
        if(!GameObject.Find("HeroCharacter")) {
            GameObject hero = Instantiate(heroCharacter, nextHeroPosition, Quaternion.identity) as GameObject;
            hero.name = "HeroCharacter"; 
        }
*/
        instance.name = "Game Manager";
        gui = GameObject.Find("BattleUI");
        gui.SetActive(false);
    }

    public void LoadNextScene(string sceneToLoad) {
        SceneManager.LoadScene(sceneToLoad); 
    }

    public void LoadSceneAfterBattle() {
        SceneManager.LoadScene(lastScene);
    }

    public void StartBattle(int encounteredEnemies, GameObject enemy) {
        if (encounteredEnemies < 1) encounteredEnemies = 1;
        if (encounteredEnemies > 4) encounteredEnemies = 4;


        enemyAmount = encounteredEnemies;
        for(int i = 0; i < encounteredEnemies; i++) {
            enemiesToBattle.Add(enemy);
        }

        lastHeroPosition = GameObject.Find("HeroCharacter").gameObject.transform.position;
        nextHeroPosition = lastHeroPosition;
        lastScene = SceneManager.GetActiveScene().name;
        //SceneManager.LoadScene("TurnBasedFight");      
    }

    public static void PlayMusic(AudioSource audioToPlay) {
        if (currentAudio != null) {
            currentAudio.Pause();
            currentAudio.time = 0;
        }
        currentAudio = audioToPlay;
        currentAudio.Play();
    }
}
