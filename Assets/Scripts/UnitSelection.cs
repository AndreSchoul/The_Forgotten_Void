// Author: André Schoul

using System.Collections;
using UnityEngine;

public class UnitSelection : MonoBehaviour {

    public static bool setEnemySelection = false;
    public static GameObject detectedEnemy;
    public static GameObject enemySelector;


    private BattleStateMachine bsm;
    private int currentSelectedEnemyNumber;
    private bool wait = true;
    private bool wait2 = true;

    private void Start() {
        bsm = GameObject.Find("BattleManager").GetComponent<BattleStateMachine>();
        detectedEnemy = bsm.enemiesInBattle[0];
        enemySelector = detectedEnemy.transform.Find("Selector").gameObject;
        enemySelector.SetActive(true);
        currentSelectedEnemyNumber = 0;
    }

    private void Update() {
  //      SelectEnemy();

        if (Input.GetButton("Submit") && BattleStateMachine.selectEnemy) {
            BattleStateMachine.selectEnemy = false;
            bsm.ActionToPerform(BattleStateMachine.selectedAttack);
        }

        Debug.Log("buttonNumber in unitselection: " + BattleStateMachine.buttonNumber);
        //
    }

    IEnumerator Wait2(bool isLeft, bool selectEnemy) {
        wait2 = false;
        yield return new WaitForSecondsRealtime(0.75f);
        if (selectEnemy) {
            if (isLeft) {
                if (currentSelectedEnemyNumber == 0) {
                    detectedEnemy = bsm.enemiesInBattle[bsm.enemiesInBattle.Count - 1];
                    enemySelector.SetActive(false);
                    enemySelector = detectedEnemy.transform.Find("Selector").gameObject;
                    enemySelector.SetActive(true);
                    currentSelectedEnemyNumber = bsm.enemiesInBattle.Count - 1;
                } else {
                    detectedEnemy = bsm.enemiesInBattle[currentSelectedEnemyNumber - 1];
                    enemySelector.SetActive(false);
                    enemySelector = detectedEnemy.transform.Find("Selector").gameObject;
                    enemySelector.SetActive(true);
                    currentSelectedEnemyNumber--;
                }
            } else {
                if (currentSelectedEnemyNumber == bsm.enemiesInBattle.Count - 1) {
                    detectedEnemy = bsm.enemiesInBattle[0];
                    enemySelector.SetActive(false);
                    enemySelector = detectedEnemy.transform.Find("Selector").gameObject;
                    enemySelector.SetActive(true);
                    currentSelectedEnemyNumber = 0;
                } else {
                    detectedEnemy = bsm.enemiesInBattle[currentSelectedEnemyNumber + 1];
                    enemySelector.SetActive(false);
                    enemySelector = detectedEnemy.transform.Find("Selector").gameObject;
                    enemySelector.SetActive(true);
                    currentSelectedEnemyNumber++;
                }
            }
        } else {
            if (!isLeft) {
                if (BattleStateMachine.buttonNumber == 0) {
                    BattleStateMachine.buttonNumber = bsm.actionButtons.Count - 1;
                } else {
                    BattleStateMachine.buttonNumber--;
                }
            } else {
                if (BattleStateMachine.buttonNumber == bsm.actionButtons.Count - 1) {
                    BattleStateMachine.buttonNumber = 0;
                } else {
                    BattleStateMachine.buttonNumber++;
                }
            }

         //   Debug.Log("buttonNumber in unitselection: " + BattleStateMachine.buttonNumber);

        }
        wait2 = true;
 //       Debug.Log(currentSelectedEnemyNumber);
    }

    void OnMouseDown() {
        DetectTarget();
    }

    public void SelectEnemy() {
        if (PlayerController.isInFight) {
            if ((Input.GetKey(KeyCode.LeftArrow)  || Input.GetKey(KeyCode.A)) && wait2) StartCoroutine(Wait2(true, BattleStateMachine.selectEnemy));
            if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && wait2) StartCoroutine(Wait2(false, BattleStateMachine.selectEnemy));
        }
    }

    /*
    public void SelectEnemy() {
        if (PlayerController.isInFight) {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
                StartCoroutine(Wait2());
                if (currentSelectedEnemyNumber == 0) {
                    detectedEnemy = bsm.enemiesInBattle[bsm.enemiesInBattle.Count - 1];
                    enemySelector.SetActive(false);
                    enemySelector = detectedEnemy.transform.Find("Selector").gameObject;
                    enemySelector.SetActive(true);
                    currentSelectedEnemyNumber = bsm.enemiesInBattle.Count - 1;
                } else {
                    detectedEnemy = bsm.enemiesInBattle[currentSelectedEnemyNumber - 1];
                    enemySelector.SetActive(false);
                    enemySelector = detectedEnemy.transform.Find("Selector").gameObject;
                    enemySelector.SetActive(true);
                    currentSelectedEnemyNumber--;
                }
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
                StartCoroutine(Wait2());
                if (currentSelectedEnemyNumber == bsm.enemiesInBattle.Count - 1) {
                    detectedEnemy = bsm.enemiesInBattle[0];
                    enemySelector.SetActive(false);
                    enemySelector = detectedEnemy.transform.Find("Selector").gameObject;
                    enemySelector.SetActive(true);
                    currentSelectedEnemyNumber = 0;
                } else {
                    detectedEnemy = bsm.enemiesInBattle[currentSelectedEnemyNumber + 1];
                    enemySelector.SetActive(false);
                    enemySelector = detectedEnemy.transform.Find("Selector").gameObject;
                    enemySelector.SetActive(true);
                    currentSelectedEnemyNumber++;
                }
            }
        }
    }
    */
    public static void DetectTarget() {
        //if(JNRCharacterController.isInFight) { 
        if (PlayerController.isInFight) {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, GameObject.Find("Main Camera").transform.position.z * -1)), Vector2.zero);
            if (hit.collider != null) {
                if (hit.collider.gameObject.tag == "Enemy") {
                    if (detectedEnemy == null) {
                        detectedEnemy = hit.collider.gameObject;
                        enemySelector = detectedEnemy.transform.Find("Selector").gameObject;
                        enemySelector.SetActive(true);
                        setEnemySelection = true;
                    } else {
                        if (detectedEnemy == hit.collider.gameObject) {
                            enemySelector = detectedEnemy.transform.Find("Selector").gameObject;
                            if (!setEnemySelection) {
                                enemySelector.SetActive(true);
                                setEnemySelection = true;
                            } else {
                                enemySelector.SetActive(false);
                                setEnemySelection = false;
                            }
                        } else {
                            enemySelector = detectedEnemy.transform.Find("Selector").gameObject;
                            enemySelector.SetActive(false);
                            detectedEnemy = hit.collider.gameObject;
                            enemySelector = detectedEnemy.transform.Find("Selector").gameObject;
                            enemySelector.SetActive(true);
                            setEnemySelection = true;
                        }
                    }
                }
            }
        }
    }
}