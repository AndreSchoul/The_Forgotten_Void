using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBlaster : BaseAttack
{
    public ShootBlaster()
    {
        attackName = "Shoot Blaster";
        attackDescription = "A mysterious Blaster which shoots lasers!";
        attackDamage = 20f;
        attackCost = 2f;
        attackType = AttackType.range;
    }
}
