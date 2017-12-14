// Author: André Schoul

using UnityEngine;

public class UnitSelection : MonoBehaviour {

    public static bool setEnemySelection = false;
    public static GameObject detectedEnemy;
    public static GameObject enemySelector;

    void OnMouseEnter() {
        DetectTarget();
    }

    void OnMouseDown() {
        DetectTarget();
    }

    public static void DetectTarget() {
        if(JNRCharacterController.isInFight) { 
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