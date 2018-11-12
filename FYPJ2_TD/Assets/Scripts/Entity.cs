using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    [SerializeField] protected int i_health, i_attackDmg, i_attackSpeed, i_moveSpeed;
    [SerializeField] protected string s_name;
    [SerializeField] protected GameObject[] target;
    [SerializeField] protected GameObject talkBuddy;

    public Entity() //constructor
    {
        i_health = i_attackDmg = i_attackSpeed = i_moveSpeed = 0;
        s_name = "noname";
    }

    public void Talk()
    {
        Debug.Log(s_name + " is happy");
    }

    

    public void SetHealth(int health){  i_health = health;  }
    public int GetHealth() { return i_health; }

    public void SetAttackDmg(int dmg) { i_attackDmg= dmg; }
    public int GetAttackDmg() { return i_attackDmg; }

    public void SetAttackSpeed(int atkSpeed) { i_attackSpeed = atkSpeed; }
    public int GetAttackSpeed() { return i_attackSpeed; }

    public void SetMoveSpeed(int moveSpeed) { i_moveSpeed = moveSpeed; }
    public int GetMoveSpeed() { return i_moveSpeed; }

    public void SetName(string name) { s_name = name; }
    public string GetName(){    return s_name; }
}
