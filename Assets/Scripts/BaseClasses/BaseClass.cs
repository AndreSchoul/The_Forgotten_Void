// Author: André Schoul

using System.Collections.Generic;

[System.Serializable]
public class BaseClass {

    // base attributes
    public string name_;
    public float baseHP;
    public float baseMP;
    public float baseATK;
    public float baseDEF;
    public float currentHP;
    public float currentMP;
    public float currentATK;
    public float currentDEF;
    public int level;
    public List<BaseAttack> attacks = new List<BaseAttack>();
    public List<BaseAttack> actionPointAttacks = new List<BaseAttack>();

    // optional attributes which may or may not be implemented due to time management
    public int luck;
    public int stamina;
    public int agility;
    public int dexterity;
    public int intelligence;
    public float criticalHitChance;
}
