// Author: André Schoul

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleStateMachine : MonoBehaviour {

    public enum PerformAction {
        wait,
        takeAction,
        performAction,
        checkAlive,
        won,
        lost
    }

    public enum HeroGUI {
        activate,
        wait,
        done
    }

    public PerformAction battleStates;
    public List<HandleTurns> performList = new List<HandleTurns>();
    public List<GameObject> herosInBattle = new List<GameObject>();
    public List<GameObject> enemiesInBattle = new List<GameObject>();
    public List<GameObject> herosToManage = new List<GameObject>();
    public GameObject attackPanel;
    public GameObject heroStats;
    public GameObject actionButton;
    public Transform actionSpacer;

    public HeroGUI heroInput;
    public List<GameObject> actionButtons = new List<GameObject>();
    private HandleTurns heroChoice;
    public bool createAttackPanelOnce = true;

    public GameObject detectedTarget;


    private static bool fightStartet = true;

    private bool setFirstEnemy = true;


    private bool fightWasBoss = false;
    private bool fightOver = true;

    private GameObject battleBox;

    public static bool selectEnemy = false;
    private bool ColorUp = true;
    public static int buttonNumber;
    public static BaseAttack selectedAttack;

    // Use this for initialization
    void Start() {
        battleStates = PerformAction.wait;
        heroInput = HeroGUI.activate;
        attackPanel.SetActive(false);
        heroStats.SetActive(false);

        battleBox = new GameObject();
        battleBox.name = "BattleBox";

        buttonNumber = 0;
    }

    // Update is called once per frame
    void Update() {
        //if (JNRCharacterController.isInFight) {
        if (PlayerController.isInFight) {
            Debug.Log("buttonNumber: " + buttonNumber);
            //Debug.Log("actionbuttons: " + actionButtons.Count);
            if (setFirstEnemy) {

                if (UnitSelection.detectedEnemy.name == "Battle_Robot_1") fightWasBoss = true;

                detectedTarget = enemiesInBattle[0];
                setFirstEnemy = false;
            }
            switch (battleStates) {
                case (PerformAction.wait):
                    if (performList.Count > 0) {
                        battleStates = PerformAction.takeAction;
                    }
                    break;
                case (PerformAction.takeAction):
                    GameObject performer = GameObject.Find(performList[0].attacker);
                    // if it is an enemy
                    if (performList[0].type == "Enemy") {
                        CharacterStateMaschine esm = performer.GetComponent<CharacterStateMaschine>();
                        for (int i = 0; i < herosInBattle.Count; i++) {
                            if (performList[0].attackersTarget == herosInBattle[i]) {
                                esm.enemyToAttack = performList[0].attackersTarget;
                                esm.currentState = CharacterStateMaschine.TurnState.action;
                                break;
                            } else {
                                performList[0].attackersTarget = herosInBattle[Random.Range(0, herosInBattle.Count)];
                                esm.enemyToAttack = performList[0].attackersTarget;
                                esm.currentState = CharacterStateMaschine.TurnState.action;
                            }
                        }
                    }
                    // if it is a hero
                    if (performList[0].type == "Hero") {
                        CharacterStateMaschine hsm = performer.GetComponent<CharacterStateMaschine>();
                        hsm.enemyToAttack = performList[0].attackersTarget;
                        hsm.currentState = CharacterStateMaschine.TurnState.action;
                    }
                    battleStates = PerformAction.performAction;
                    break;
                case (PerformAction.performAction):
                    break;
                case (PerformAction.checkAlive):
                    if (herosInBattle.Count < 1) {
                        // loose battle
                        battleStates = PerformAction.lost;
                    } else if (enemiesInBattle.Count < 1) {
                        // win battle
                        battleStates = PerformAction.won;
                    } else {
                        ClearAttackPanels();
                        heroInput = HeroGUI.activate;
                    }
                    break;
                case (PerformAction.won):
                    ResetAfterBattle(true);
                    break;
                case (PerformAction.lost):
                    ResetAfterBattle(false);
                    break;
            }
            switch (heroInput) {
                case (HeroGUI.activate):
                    if (battleStates == PerformAction.won || battleStates == PerformAction.lost) {
                        break;
                    }
                    if (CharacterStateMaschine.herosTurnOver == herosInBattle.Count && Input.GetButton("Submit")) {
                        Debug.Log("It's not your turn!");
                        break;
                    }
                    if (herosToManage.Count > 0) {

                        herosToManage[0].GetComponent<CharacterStateMaschine>().ToggleVisibilityHeroStats(true);


                        herosToManage[0].transform.Find("Selector").gameObject.SetActive(true);
                        heroChoice = new HandleTurns();
                        if (Input.GetButton("Submit") || fightStartet) {
                    //    if(!CharacterStateMaschine.isEnemiesTurn) { 
                            fightStartet = false;
                            //attackPanel.SetActive(true);
                            if (createAttackPanelOnce) {


                                attackPanel.SetActive(true);


                                CreateActionButton();
                                createAttackPanelOnce = false;


                                heroInput = HeroGUI.wait;
                            }
                            // heroInput = HeroGUI.wait;
                        }
                    }



                    /*
                    if (Input.GetButton("Submit") && selectEnemy) {
                        BattleStateMachine.selectEnemy = false;
                        ActionToPerform(BattleStateMachine.selectedAttack);
                    }*/




                    break;
                case (HeroGUI.wait):
                    // idle
                    break;
                case (HeroGUI.done):
                    HeroInputDone();
                    break;
            }



       //     SwitchActionButtons();
        }
    }

    /// <summary>
    /// Stores enemies action in "performList" list.
    /// </summary>
    /// <param name = "input"> Enemies input data</param>
    public void CollectActions(HandleTurns input) {
        performList.Add(input);
    }

    /// <summary>
    /// Performs a heros chosen action.
    /// </summary>
    /// <param name = "atk">Attack from an attack button which a hero chose</param>
    public void ActionToPerform(BaseAttack atk) {
        heroChoice.attacker = herosToManage[0].name;
        heroChoice.attackersGameobject = herosToManage[0];
        heroChoice.type = "Hero";
        heroChoice.chosenAttack = atk;
        // check if an enemy is selected
        if (UnitSelection.detectedEnemy == null /*|| UnitSelection.setEnemySelection == false*/) {
            Debug.Log("Select an enemy to attack!");
            heroInput = HeroGUI.activate;



        } else {
            attackPanel.SetActive(false);
            SelectedTarget(UnitSelection.detectedEnemy);

            detectedTarget = UnitSelection.detectedEnemy;

            AudioManager audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            audioManager.PlaySound("gui");
           
        }
        //createAttackPanelOnce = true;
    }

    /// <summary>
    /// Chooses target to interact with.
    /// </summary>
    /// <param name = "choosenEnemy">Selected target</param>
    public void SelectedTarget(GameObject choosenEnemy) {
        heroChoice.attackersTarget = choosenEnemy;
        heroInput = HeroGUI.done;
    }

    /// <summary>
    /// Adds "heroChoice" to the performList, clears attack panels, deselects active selector, removes hero from "herosToManage" list and changes heroInput state to "activate".
    /// </summary>
    private void HeroInputDone() {
        performList.Add(heroChoice);
        ClearAttackPanels();
        herosToManage[0].transform.Find("Selector").gameObject.SetActive(false);
        herosToManage[0].GetComponent<CharacterStateMaschine>().ToggleVisibilityHeroStats(false);
        herosToManage.RemoveAt(0);
        heroInput = HeroGUI.activate;


        selectedAttack = null;
        buttonNumber = 0;
    }

    /// <summary>
    /// Clears attack panels and corresponding buttons.
    /// </summary>
    private void ClearAttackPanels() {
              UnitSelection.DetectTarget();
        UnitSelection.detectedEnemy = null;
        if (UnitSelection.enemySelector != null) {
            UnitSelection.enemySelector.SetActive(false);
        }
        foreach (GameObject actionBtn in actionButtons) {
            Destroy(actionBtn);
        }
        actionButtons.Clear();
    }

    /// <summary>
    /// Creates one button for each character's available attack.
    /// </summary>
    public void CreateActionButton() {
        foreach (BaseAttack action in herosToManage[0].GetComponent<CharacterStateMaschine>().baseClass.actionPointAttacks) {
            GameObject actionButton_ = Instantiate(actionButton) as GameObject;
            Text actionButtonText_ = actionButton_.transform.Find("Text").GetComponent<Text>();
            actionButtonText_.text = action.attackName;
            ActionButton actionBtn = actionButton_.GetComponent<ActionButton>();
            actionBtn.attackToPerform = action;
            actionButton_.transform.SetParent(actionSpacer, false);
            actionButtons.Add(actionButton_);
            if (action.name == "Attack_Standard") actionButton_.GetComponent<Button>().image.sprite = Resources.Load<Sprite>("GUI/normal-attack-icon");
            if (action.name == "Attack_TripleHit") actionButton_.GetComponent<Button>().image.sprite = Resources.Load<Sprite>("GUI/double-attack-icon");
            if (action.name == "Blaster") actionButton_.GetComponent<Button>().image.sprite = Resources.Load<Sprite>("GUI/weapon-attack-icon");
            actionButton_.GetComponent<Button>().onClick.AddListener(() => ActionToPerform(action));
        }
    }

    /// <summary>
    /// Spawns characters (heros or enemies) in dependency on stated parameters.
    /// </summary>
    /// <param name ="characterCount">Amount of characters to be spawned</param>
    /// <param name ="charactersFolderLocation">List with the prefabs character (folder location) to spawn</param>
    /// <param name ="spawnpoints">Spawnpoint of each particular character (Spawnpoints attached to camera)</param>
    /// <param name ="isHero"><para>True - character is a hero</para><para>False - character is an enemy</para></param>
    public void SpawnCharacters(int characterCount, List<string> charactersFolderLocation, List<Transform> spawnpoints, bool isHero) {
        if (characterCount < 1) characterCount = 1;
        if (characterCount > 4) characterCount = 4;
        GameObject newCharacter = null;
        int i = 0;
        foreach (string s in charactersFolderLocation) {
            if (charactersFolderLocation.Count == 1) {
                newCharacter = Instantiate(Resources.Load<GameObject>(s), spawnpoints[i + 0].position, Quaternion.identity);
            } else if (charactersFolderLocation.Count == 2) {
                newCharacter = Instantiate(Resources.Load<GameObject>(s), spawnpoints[i + 1].position, Quaternion.identity);
            } else if (charactersFolderLocation.Count == 3) {
                newCharacter = Instantiate(Resources.Load<GameObject>(s), spawnpoints[i + 3].position, Quaternion.identity);
            } else if (charactersFolderLocation.Count == 4) {
                newCharacter = Instantiate(Resources.Load<GameObject>(s), spawnpoints[i + 6].position, Quaternion.identity);
            }
            i++;
            newCharacter.name = newCharacter.GetComponent<CharacterStateMaschine>().baseClass.name_ + "_" + i;
            newCharacter.GetComponent<CharacterStateMaschine>().baseClass.name_ = newCharacter.name;
            if (isHero) {
                herosInBattle.Add(newCharacter);
            } else {
                enemiesInBattle.Add(newCharacter);
            }

            if (!isHero) {
                newCharacter.GetComponent<CharacterStateMaschine>().enemyNumber = i;
                if (newCharacter.GetComponent<CharacterStateMaschine>().enemyNumber > CharacterStateMaschine.lastEnemey) CharacterStateMaschine.lastEnemey = newCharacter.GetComponent<CharacterStateMaschine>().enemyNumber;
            }
            newCharacter.transform.SetParent(battleBox.transform);
        }
    }

    /// <summary>
    /// Resets all relevant battle data depending on who won the fight e.g. music, whose turn it was and gameobjects themself.
    /// </summary>
    /// <param name = "won"><para>True - Hero(s) won the game</para><para>False - Enemies won the game</para></param>
    public void ResetAfterBattle(bool won) {
        //JNRCharacterController.isInFight = false;
        StartCoroutine(Wait(2f, won));
        /*     PlayerController.isInFight = false;
             AudioSource audio = GetComponent<AudioSource>();
             audio.Pause();
             audio.time = 0;
             if (won) {
                 Debug.Log("You won the battle!");
                 Destroy(GameManager.instance.collidedEnemy);

             } else {
                 Debug.Log("You lost the battle!");
                 // TODO send player back to start/checkpoint/space station or alike
             }
             foreach (GameObject go in herosInBattle) {
                 Destroy(go);
                 Destroy(go.GetComponent<CharacterStateMaschine>().healthPanel);
                 Destroy(go.GetComponent<CharacterStateMaschine>().heroStats);
             }
             CharacterStateMaschine.enemiesFinishedTurn = 0;
             CharacterStateMaschine.enemiesTurnOver = 0;
             CharacterStateMaschine.herosTurnOver = 0;
             CharacterStateMaschine.isEnemiesTurn = false;
             CharacterStateMaschine.lastEnemey = 0;
             createAttackPanelOnce = true;
             herosInBattle.Clear();
             foreach (GameObject go in enemiesInBattle) {
                 Destroy(go);
             }
             enemiesInBattle.Clear();
             foreach (GameObject go in herosToManage) {
                 Destroy(go);
             }
             herosToManage.Clear();
             performList.Clear();
             //JNRCharacterController.isInFight = false;
             //JNRCharacterController.hero.SetActive(true);
             PlayerController.isInFight = false;
             fightStartet = true;
             PlayerController.hero.SetActive(true);
             battleStates = PerformAction.wait;
             heroInput = HeroGUI.activate;
             GameManager.instance.gui.SetActive(false);

             GameObject cam = GameObject.Find("Main Camera");
             Quaternion rot = cam.transform.rotation;
             rot.y = 0f;
             cam.transform.rotation = rot;

             //GameManager.PlayMusic(JNRCharacterController.audio_JnR);
             GameManager.PlayMusic(PlayerController.audio_JnR);

             setFirstEnemy = true;

             foreach(GameObject go in enemiesInBattle) {
                 Destroy(go);
             }*/


        foreach (GameObject go in enemiesInBattle) {
            Debug.Log(go.name);
            Destroy(go);
            Destroy(go.GetComponent<CharacterStateMaschine>().healthPanel);
        }
        foreach (GameObject go in herosInBattle) {
            // Destroy(go);
            Destroy(go.GetComponent<CharacterStateMaschine>().healthPanel);
            Destroy(go.GetComponent<CharacterStateMaschine>().heroStats);
        }
        enemiesInBattle.Clear();
        /*
        AudioSource audio = GetComponent<AudioSource>();
        audio.Pause();
        audio.time = 0;
        GameManager.PlayMusic(PlayerController.audio_JnR);      */
    }

    IEnumerator Wait(float time, bool won) {

        if (fightOver) {
            fightOver = false;

            GameObject.Find("Blende").GetComponent<BlendenController>().blenden();
            /*
            PlayerController.hero.SetActive(true);
            */
            AudioSource audio = GetComponent<AudioSource>();
            audio.Pause();
            audio.time = 0;/*
        GameManager.PlayMusic(PlayerController.audio_JnR);
        */
            yield return new WaitForSeconds(time);

            PlayerController.isInFight = false;
            /*      AudioSource audio = GetComponent<AudioSource>();
                  audio.Pause();
                  audio.time = 0;     */
            if (won) {
                Debug.Log("You won the battle!");
                Destroy(GameManager.instance.collidedEnemy);

            } else {
                Debug.Log("You lost the battle!");
                // TODO send player back to start/checkpoint/space station or alike
            }
            foreach (GameObject go in herosInBattle) {
                Destroy(go);
                //           Destroy(go.GetComponent<CharacterStateMaschine>().healthPanel);
                //           Destroy(go.GetComponent<CharacterStateMaschine>().heroStats);
            }
            CharacterStateMaschine.enemiesFinishedTurn = 0;
            CharacterStateMaschine.enemiesTurnOver = 0;
            CharacterStateMaschine.herosTurnOver = 0;
            CharacterStateMaschine.isEnemiesTurn = false;
            CharacterStateMaschine.lastEnemey = 0;
            createAttackPanelOnce = true;
            herosInBattle.Clear();
            foreach (GameObject go in enemiesInBattle) {
                Destroy(go);
            }
            enemiesInBattle.Clear();
            foreach (GameObject go in herosToManage) {
                Destroy(go);
            }
            herosToManage.Clear();
            performList.Clear();
            //JNRCharacterController.isInFight = false;
            //JNRCharacterController.hero.SetActive(true);
            PlayerController.isInFight = false;
            fightStartet = true;
            //      PlayerController.hero.SetActive(true);
            battleStates = PerformAction.wait;
            heroInput = HeroGUI.activate;
            GameManager.instance.gui.SetActive(false);

            GameObject cam = GameObject.Find("Main Camera");
            Quaternion rot = cam.transform.rotation;
            rot.y = 0f;
            cam.transform.rotation = rot;

            //GameManager.PlayMusic(JNRCharacterController.audio_JnR);


            setFirstEnemy = true;

            PlayerController.hero.SetActive(true);
            //      AudioSource audio = GetComponent<AudioSource>();
            audio.Pause();
            audio.time = 0;
            GameManager.PlayMusic(PlayerController.audio_JnR);


            if (fightWasBoss) {
                GameManager gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
                gm.LoadNextScene("Spacestation");
            }

            foreach (Transform go in battleBox.transform) {
                Destroy(go.gameObject);
            }

            fightOver = true;
        }
    }

    private void SwitchActionButtons() {
        if (actionButtons.Count != 0) {
            if (!selectEnemy) {
                ColorBlock color = actionButtons[buttonNumber].GetComponent<Button>().colors;
                if (color.colorMultiplier <= 3f && ColorUp) color.colorMultiplier += 0.05f;
                else ColorUp = false;
                if (color.colorMultiplier >= 1 && !ColorUp) color.colorMultiplier -= 0.05f;
                else ColorUp = true;
                actionButtons[buttonNumber].GetComponent<Button>().colors = color;

                for (int i = 0; i < actionButtons.Count; i++) {
                    if (i != buttonNumber) {
                        ColorBlock currentColor = actionButtons[i].GetComponent<Button>().colors;
                        currentColor.colorMultiplier = 1;
                        actionButtons[i].GetComponent<Button>().colors = currentColor;
                    }
                }
            }
            if (Input.GetButton("Submit") && !selectEnemy) {
                StartCoroutine(WaitAfterInput());/*
                selectEnemy = true;
                selectedAttack = actionButtons[buttonNumber].GetComponent<ActionButton>().attackToPerform;
                Debug.Log(selectedAttack.name);*/
            }
        }
    }

    private IEnumerator WaitAfterInput() {
        yield return new WaitForSeconds(1);
        selectEnemy = true;
        selectedAttack = actionButtons[buttonNumber].GetComponent<ActionButton>().attackToPerform;
        Debug.Log(selectedAttack.name);
    }
}
