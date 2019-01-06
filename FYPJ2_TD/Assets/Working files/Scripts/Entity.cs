using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    [SerializeField] protected float f_health, f_maxHealth, f_attackDmg, f_attackSpeed, f_moveSpeed;
    [SerializeField] protected string s_name;
    [SerializeField] protected List<GameObject> targets;          //targets that are detected in trigger collider
    [SerializeField] protected List<GameObject> attackingTargets; //targets thats going to be attacked

    public Entity() //constructor
    {
        f_health = f_maxHealth = f_attackDmg = f_attackSpeed = f_moveSpeed = 0.0f;
        s_name = "noname";
        targets = new List<GameObject>();
        attackingTargets = new List<GameObject>();
    }

    public void DecreaseHealth(float dmg)
    {
        if (f_health > 0.0f)
            f_health -= dmg;
        if (f_health < 0.0f)
            f_health = 0.0f;
    }

    public void IncreaseHealth(float heal)
    {
        if (f_health < f_maxHealth)
            f_health += heal;
        if (f_health > f_maxHealth)
            f_health = f_maxHealth;
    }

    public void SetHealth(float health){  f_health = health;  }
    public float GetHealth() { return f_health; }

    public void SetAttackDmg(float dmg) { f_attackDmg= dmg; }
    public float GetAttackDmg() { return f_attackDmg; }

    public void SetAttackSpeed(float atkSpeed) { f_attackSpeed = atkSpeed; }
    public float GetAttackSpeed() { return f_attackSpeed; }

    public void SetMoveSpeed(float moveSpeed) { f_moveSpeed = moveSpeed; }
    public float GetMoveSpeed() { return f_moveSpeed; }

    public void SetName(string name) { s_name = name; }
    public string GetName(){    return s_name; }

    public List<GameObject> GetTargets() { return targets; }
    public List<GameObject> GetAttackingTargets() { return attackingTargets; }
}
