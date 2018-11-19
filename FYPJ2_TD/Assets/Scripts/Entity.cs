using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    [SerializeField] protected int i_health, i_maxHealth, i_attackDmg, i_attackSpeed;
    [SerializeField] protected float i_moveSpeed;  //todo- movespeed, atkspeed to float
    [SerializeField] protected string s_name;
    [SerializeField] protected List<GameObject> targets;          //targets that are detected in trigger collider
    [SerializeField] protected List<GameObject> attackingTargets; //targets thats going to be attacked

    public Entity() //constructor
    {
        i_health = i_maxHealth = i_attackDmg = i_attackSpeed = 0;
        i_moveSpeed = 0.0f;
        s_name = "noname";
        targets = new List<GameObject>();
        attackingTargets = new List<GameObject>();
    }

    public void DecreaseHealth(int dmg)
    {
        if (i_health > 0)
            i_health -= dmg;
        if (i_health < 0)
            i_health = 0;
    }

    public void IncreaseHealth(int heal)
    {
        if (i_health < i_maxHealth)
            i_health += heal;
        if (i_health > i_maxHealth)
            i_health = i_maxHealth;
    }

    public void SetHealth(int health){  i_health = health;  }
    public int GetHealth() { return i_health; }

    public void SetAttackDmg(int dmg) { i_attackDmg= dmg; }
    public int GetAttackDmg() { return i_attackDmg; }

    public void SetAttackSpeed(int atkSpeed) { i_attackSpeed = atkSpeed; }
    public int GetAttackSpeed() { return i_attackSpeed; }

    public void SetMoveSpeed(int moveSpeed) { i_moveSpeed = moveSpeed; }
    public float GetMoveSpeed() { return i_moveSpeed; }

    public void SetName(string name) { s_name = name; }
    public string GetName(){    return s_name; }

    public List<GameObject> GetTargets() { return targets; }
    public List<GameObject> GetAttackingTargets() { return attackingTargets; }
}
