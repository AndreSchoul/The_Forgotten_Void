using System.Collections;
using UnityEngine;

[System.Serializable]
public class HandleTurns
{
    public string type;
    public string attacker;                // name of the one who attacks
    public GameObject attackersGameobject; // the gameobject which attacks
    public GameObject attackersTarget;      // the object which is going to be attacked
    public BaseAttack chosenAttack;
}
