using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_TripleHit : BaseAttack
{
    public Attack_TripleHit()
    {
        attackName = "Triple Hit";
        attackDescription = "A powerful attack which hits three times";
        attackDamage = 30f;
        attackCost = 3f;
        attackType = AttackType.melee;
    }
}
