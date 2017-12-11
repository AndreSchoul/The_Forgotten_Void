using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Standard : BaseAttack
{ 
    public Attack_Standard()
    {
        attackName = "Standard Attack";
        attackDescription = "Standard Attack";
        attackDamage = 10f;
        attackCost = 0f;
        attackType = AttackType.melee;
    }
}
