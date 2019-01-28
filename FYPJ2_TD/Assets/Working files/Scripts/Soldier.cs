using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Entity
{
    [HideInInspector]
    public StateMachine sm;
    private int numOfTargets = 1;

    // Use this for initialization
    void Start()
    {
        s_name = "Soldier";
        f_health = 1.0f;
        f_attackDmg = 1.0f;
        f_attackSpeed = 1.0f;
        f_moveSpeed = 1.0f;

        sm = new StateMachine();
        sm.AddState(new SoldierChase("Chase", this));
        sm.AddState(new SoldierIdle("Idle", this));
        sm.AddState(new SoldierAttack("Attack", this));
    }

    // Update is called once per frame
    void Update()
    {
        sm.Update();
        SetTarget();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            targets.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            targets.Remove(other.gameObject);
            attackingTargets.Remove(other.gameObject);
        }
    }

    public void SetTarget()
    {
        if (targets.Count != 0 && attackingTargets.Count < numOfTargets)
        {
            for (int i = 0; i < numOfTargets; ++i)
            {
                attackingTargets.Add(targets[i]);
            }
        }
    }
}
