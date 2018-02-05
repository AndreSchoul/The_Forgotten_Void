// Author: André Schoul

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStateMaschine : MonoBehaviour {

    public enum TurnState {
        wait,
        action,
        attack,
        turnOver,
        dead
    }

    public TurnState currentState;
    public BaseClass baseClass;
    public GameObject selector;
    public GameObject healthPanel;
    public GameObject heroToAttack;
    public GameObject enemyToAttack;
    public GameObject heroStats;
    public static int herosTurnOver = 0;
    public static int enemiesTurnOver = 0;
    public static int enemiesFinishedTurn = 0;
    public static bool isEnemiesTurn = false;

    private Animator animator;
    private Vector3 startposition;
    //private JNRCharacterController characterWalk;
    private PlayerController characterWalk;
    private BattleStateMachine bsm;
    private UnitStats_Health stats;
    private Transform healthPanelTransform;
    private float animationSpeed = 75f;
    private bool alive = true;
    private bool doOnce = false;
    private bool actionStarted = false; 
    private static bool herosDoOnce = true;
    private static bool enemiesDoOnce = false;

    private GameObject cam;
    private bool doWait = true;

    public static int lastEnemey = 0;
    public int enemyNumber = 0;

    // Use this for initialization
    void Start () {

        cam = GameObject.Find("Main Camera");
        StartCoroutine(RotateCamera(Vector3.down * -1f, 0.5f)); 
        animator = this.gameObject.GetComponentInChildren<Animator>();
        selector.SetActive(false);
        if (this.gameObject.tag == "Hero") {
            //characterWalk = this.gameObject.GetComponent<JNRCharacterController>();
            characterWalk = this.gameObject.GetComponent<PlayerController>();

            //this.gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        //if (JNRCharacterController.isInFight) {
        if (PlayerController.isInFight) {
            currentState = TurnState.wait;
            startposition = transform.position;
            CreateHealthPanel();
            bsm = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();

            if(this.gameObject.tag == "Hero") CreateHeroPanel();
        }
    }

    // Update is called once per frame
    void Update() {
        switch (currentState) {
            case (TurnState.wait):
                //if (JNRCharacterController.isInFight) {
                if (PlayerController.isInFight) {
                    animator.SetFloat("x", 0);
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isIdling", true);
                    animator.SetBool("isAttacking", false);

                    if (!bsm.herosToManage.Contains(this.gameObject) && this.gameObject.tag == "Hero") {
                        bsm.herosToManage.Add(this.gameObject);
                    }
                    if (this.gameObject.tag == "Enemy") {
                        if (herosTurnOver == bsm.herosInBattle.Count && enemiesTurnOver < bsm.enemiesInBattle.Count) {
                            isEnemiesTurn = true;
                            ChooseAction();
                            enemiesTurnOver++;
                        }
                    }
                }
                break;
            case (TurnState.action):

                animator.SetFloat("x", 0);
                animator.SetBool("isWalking", true);
                animator.SetBool("isIdling", false);
                animator.SetBool("isAttacking", false);

                StartCoroutine(TimeForAction());
                break;
            case (TurnState.attack):

                animator.SetBool("isWalking", false);
                animator.SetBool("isIdling", false);
                animator.SetBool("isAttacking", true);

                break;
            case (TurnState.turnOver):
                if (doOnce && this.gameObject.tag == "Hero" && herosTurnOver < bsm.herosInBattle.Count) {
                    herosTurnOver++;
                    doOnce = false;
                }
                if (this.gameObject.tag == "Enemy") {
                    foreach (GameObject go in bsm.enemiesInBattle) {
                        if (this.gameObject.transform.position == startposition) {
                            enemiesFinishedTurn++;
                        }
                    }
                    if (enemiesFinishedTurn == bsm.enemiesInBattle.Count * bsm.enemiesInBattle.Count) {
                        herosTurnOver = 0;
                        enemiesTurnOver = 0;
                        enemiesFinishedTurn = 0;
                        isEnemiesTurn = false;


                        StartCoroutine(RotateCamera(Vector3.down * -2f, 0.5f));
                    }
                }


                if (herosTurnOver == bsm.herosInBattle.Count && enemiesFinishedTurn == 0) {
                    StartCoroutine(RotateCamera(Vector3.down * 2f, 0.5f));
                }
                /*
                if (doWait) {
                    doWait = false;
                    StartCoroutine(Wait());                 
                }*/
                currentState = TurnState.wait;

                break;
            case (TurnState.dead):
                if (!alive) {
                    return;
                } else if (this.gameObject.tag == "Hero") {
                    // change tag of the hero
                    this.gameObject.tag = "DeadHero";
                    // not attackable anymore
                    bsm.herosInBattle.Remove(this.gameObject);
                    // disable input
                    bsm.herosToManage.Remove(this.gameObject);
                    // reset GUI
                    bsm.attackPanel.SetActive(false);
                } else {
                    // change tag of the enemy
                    this.gameObject.tag = "DeadEnemy";
                    // not attackable anymore
                    bsm.enemiesInBattle.Remove(this.gameObject);
                }
                // disable selector
                selector.SetActive(false);
                // remove item from perfomList
                for (int i = 0; i < bsm.performList.Count; i++) {
                    if (bsm.performList[i].attackersGameobject == this.gameObject) {
                        bsm.performList.Remove(bsm.performList[i]);
                    }
                }
                // change color / play animation
                //this.gameObject.GetComponent<MeshRenderer>().material.color = new Color32(105, 105, 105, 255);
                // reset hero input
                //bsm.heroInput = BattleStateMachine.HeroGUI.activate;
                bsm.battleStates = BattleStateMachine.PerformAction.checkAlive;
                alive = false;
                Destroy(this.gameObject);
                Destroy(healthPanel);
                break;
        }
    }

    /// <summary>
    /// Lets the enemy choose a skill and a target to use on.
    /// </summary>
    private void ChooseAction() {
        if (bsm.herosInBattle.Count > 0) {
            HandleTurns chosenAction = new HandleTurns();
            chosenAction.type = "Enemy";
            chosenAction.attacker = baseClass.name_;
            chosenAction.attackersGameobject = this.gameObject;
            chosenAction.attackersTarget = bsm.herosInBattle[Random.Range(0, bsm.herosInBattle.Count)];
            chosenAction.chosenAttack = baseClass.attacks[Random.Range(0, baseClass.attacks.Count)];
            bsm.CollectActions(chosenAction);
        }
    }

    /// <summary>
    /// Character takes damage and checks if it was lethal.
    /// </summary>
    /// <param name = "dmg">Damage amount taken</param>
    private void TakeDmg(float dmg) {
        baseClass.currentHP -= dmg;
        if (baseClass.currentHP <= 0) {
            baseClass.currentHP = 0;
            currentState = TurnState.dead;
        }
        DamagePopUp_Controller.CreateDamagePopUp_Text(dmg.ToString(), transform);
        UpdateHealthPanel();
    }

    /// <summary>
    /// Deals damage to its chosen target.
    /// </summary>
    private void DoDmg() {
        float calculated_Dmg = baseClass.currentATK + bsm.performList[0].chosenAttack.attackDamage +
        (baseClass.currentATK + bsm.performList[0].chosenAttack.attackDamage) * (baseClass.level / 100f * 3f);
        enemyToAttack.GetComponent<CharacterStateMaschine>().TakeDmg(calculated_Dmg);
    }

    /// <summary>
    /// Creates characters health panel which represents his health points.
    /// </summary>
    private void CreateHealthPanel() {
        healthPanel = Instantiate(healthPanel).gameObject;
        stats = healthPanel.GetComponent<UnitStats_Health>();
        stats.healthBar.value = baseClass.currentHP / baseClass.baseHP;
        stats.attackPoints = baseClass.currentMP;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x + 200, transform.position.y - 5, transform.position.z) + Camera.main.transform.position);
        healthPanel.transform.SetParent(GameObject.Find("BattleUI").transform, false);
        healthPanel.transform.position = screenPosition;
    }

    /// <summary>
    /// Updates characters health panel after it got attacked.
    /// </summary>
    private void UpdateHealthPanel() {
        stats.healthBar.value = baseClass.currentHP / baseClass.baseHP;
        stats.attackPoints = baseClass.currentMP;
    }

    /// <summary>
    /// Executes the input orders given, changes states and plays a sound when attacking.
    /// </summary>
    private IEnumerator TimeForAction() {
        if (actionStarted) {
            yield break;
        }
        actionStarted = true;
        // move character which attacks to its target
        Vector3 enemyPosition;
        if(this.gameObject.tag == "Hero") {
            enemyPosition = new Vector3(enemyToAttack.transform.position.x - 5, enemyToAttack.transform.position.y, enemyToAttack.transform.position.z);
        } else {
            enemyPosition = new Vector3(enemyToAttack.transform.position.x + 5, enemyToAttack.transform.position.y, enemyToAttack.transform.position.z);
        }



        /*
        if (this.gameObject.tag == "Hero") {
            while (MoveTowardsEnemy(new Vector3(startposition.x + 2 * cam.transform.eulerAngles.y, startposition.y, startposition.z - 2))) { yield return null; }
            while (MoveTarget(new Vector3(bsm.detectedTarget.GetComponent<CharacterStateMaschine>().startposition.x, bsm.detectedTarget.GetComponent<CharacterStateMaschine>().startposition.y, bsm.detectedTarget.GetComponent<CharacterStateMaschine>().startposition.z - 2), bsm.detectedTarget)) { yield return null; }
        }    while (MoveTowardsEnemy(new Vector3(enemyPosition.x, enemyPosition.y, enemyPosition.z - 2))) { yield return null; }*/
   



        while (MoveTowardsEnemy(enemyPosition)) { yield return null; }
        currentState = TurnState.attack;
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        // wait for the duration of the attack animation
        yield return new WaitForSeconds(0.4f);
        currentState = TurnState.action;
        // do dmg 
        DoDmg();
        // move character back to start position
        Vector3 firstPosition = startposition;


       /*
       // while (bsm.detectedTarget.GetComponent<CharacterStateMaschine>() != null) {
            while (MoveTarget(new Vector3(bsm.detectedTarget.GetComponent<CharacterStateMaschine>().startposition.x, bsm.detectedTarget.GetComponent<CharacterStateMaschine>().startposition.y, bsm.detectedTarget.GetComponent<CharacterStateMaschine>().startposition.z), bsm.detectedTarget)) { yield return null; }
       // }
       */


        while (MoveTowardsStart(firstPosition)) { yield return null; }


        if (this.gameObject.tag == "Enemy") {
            if (enemyNumber == lastEnemey) bsm.createAttackPanelOnce = true;
        } else bsm.createAttackPanelOnce = true;
        
        

        // remove this performer from the list in bsm
        bsm.performList.RemoveAt(0);
        // reset bsm -> wait
        if (bsm.battleStates != BattleStateMachine.PerformAction.won && bsm.battleStates != BattleStateMachine.PerformAction.lost) {
            bsm.battleStates = BattleStateMachine.PerformAction.wait;
        }
        if (this.gameObject.tag == "Hero") {
            doOnce = true;
        }
        currentState = TurnState.turnOver;
        // end coroutine
        actionStarted = false;
    }



    private bool MoveTarget(Vector3 target, GameObject target_) {
        return target != (target_.transform.position = Vector3.MoveTowards(target_.transform.position, target, animationSpeed * Time.deltaTime));
    }





    /// <summary>
    /// Moves the character towards the enemy.
    /// </summary>
    /// <param name = "target">Characters location to move to</param>
    private bool MoveTowardsEnemy(Vector3 target) {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animationSpeed * Time.deltaTime));
    }

    /// <summary>
    /// Moves the character towards its original battle-position.
    /// </summary>
    /// <param name = "target">Characters location to move to</param>
    private bool MoveTowardsStart(Vector3 target) {
        animator.SetFloat("x", 1);
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animationSpeed * Time.deltaTime));
    }

    /// <summary>
    /// Creates a hero panel.
    /// </summary>
    private void CreateHeroPanel() {
        heroStats = Instantiate(Resources.Load<GameObject>("Logics/HeroStats"));
        heroStats.transform.SetParent(GameObject.Find("UI_Background").transform, false);
        heroStats.SetActive(false);
        FillHeroPanel();
    }

    /// <summary>
    /// Fills the hero panel with the hero's corresponding attributes.
    /// </summary>
    private void FillHeroPanel() {
        foreach(Text txt in heroStats.transform.GetChild(0).GetChild(1).GetComponentsInChildren<Text>()) {
            if (txt.name == "Luck") txt.text = baseClass.luck.ToString();
            if (txt.name == "Stamina") txt.text = baseClass.stamina.ToString();
            if (txt.name == "Agility") txt.text = baseClass.agility.ToString();
            if (txt.name == "Dexterity") txt.text = baseClass.dexterity.ToString();
            if (txt.name == "Intelligence") txt.text = baseClass.intelligence.ToString();
            if (txt.name == "Critical Hit Chance") txt.text = baseClass.criticalHitChance.ToString();
        }
    }

    /// <summary>
    /// Toggles the visibility of the hero stats.
    /// </summary>
    /// <param name = "visibility"><para>True - hero stats are visible</para><para>False - hero stats are hidden</para></param>
    public void ToggleVisibilityHeroStats(bool visibility) {
        heroStats.SetActive(visibility);
    }

    public IEnumerator RotateCamera(Vector3 byAngles, float inTime) {
        Quaternion fromAngle = cam.transform.rotation;
        Quaternion toAngle = Quaternion.Euler(cam.transform.eulerAngles + byAngles);
        for (float t = 0f; t < 1f; t += Time.deltaTime / inTime) {
            cam.transform.rotation = Quaternion.Lerp(fromAngle, toAngle, t);
            yield return null;
        }
        //Debug.Log(cam.transform.eulerAngles.y);
    }

    IEnumerator Wait() {
        Debug.Log("Wait 10 seconds");
        yield return new WaitForSeconds(10);
        Debug.Log("okay");
        currentState = TurnState.wait;
        doWait = true;
    }
}
