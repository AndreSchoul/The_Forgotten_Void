using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseAttack : MonoBehaviour
{
    public string attackName;
    public string attackDescription;
    public float attackDamage;
    public float attackCost;
    public enum AttackType
    {
        melee,
        range,
        other
    }

    public AttackType attackType;
}
