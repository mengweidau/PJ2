﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollPatrol : State
{
    EnemyTroll thisEnemy;

    public TrollPatrol(string stateID, EnemyTroll enemy)
    {
        m_stateID = stateID;
        thisEnemy = enemy;
    }

    public override void Enter()
    {
        //Debug.Log("Patrol");
        thisEnemy.SetMoveSpeed(0.5f);
    }

    public override void Update()
    {
        Vector3 direction = thisEnemy.targetWaypoint.position - thisEnemy.transform.position;

        if (Vector3.Distance(thisEnemy.transform.position, thisEnemy.targetWaypoint.position) <= 0.1f)
        {
            thisEnemy.GetNextwaypoint();
        }
        else
        {
            thisEnemy.transform.Translate(direction.normalized * thisEnemy.GetMoveSpeed() * Time.deltaTime);
        }

        for (int i = 0; i < thisEnemy.GetAttackingTargets().Count; i++)
        {
            if (Vector3.Distance(thisEnemy.transform.position, thisEnemy.GetAttackingTargets()[i].transform.position) <= 1.0f)
            {
                thisEnemy.sm.SetNextState("Chase");
            }
        }
    }

    public override void Exit()
    {
    }
}

public class TrollChase : State
{
    EnemyTroll thisEnemy;
    Vector3 direction;

    public TrollChase(string stateID, EnemyTroll enemy)
    {
        m_stateID = stateID;
        thisEnemy = enemy;
    }

    public override void Enter()
    {
        //Debug.Log("Chase");
        thisEnemy.SetMoveSpeed(0.5f);
    }

    public override void Update()
    {
        for (int i = 0; i < thisEnemy.GetAttackingTargets().Count; i++)
        {
            if (Vector3.Distance(thisEnemy.transform.position, thisEnemy.GetAttackingTargets()[i].transform.position) > 1.0f)
            {
                thisEnemy.sm.SetNextState("Patrol");
            }
            else if (Vector3.Distance(thisEnemy.transform.position, thisEnemy.GetAttackingTargets()[i].transform.position) < 0.2f)
            {
                thisEnemy.sm.SetNextState("Attack");
            }
            else
            {
                direction = thisEnemy.GetAttackingTargets()[i].transform.position - thisEnemy.transform.position;
                thisEnemy.transform.Translate(direction.normalized * thisEnemy.GetMoveSpeed() * Time.deltaTime);
            }
        }
    }

    public override void Exit()
    {
    }
}

public class TrollAttack : State
{
    EnemyTroll thisEnemy;
    float attackCooldown =0.5f;

    public TrollAttack(string stateID, EnemyTroll enemy)
    {
        m_stateID = stateID;
        thisEnemy = enemy;
    }

    public override void Enter()
    {
        //Debug.Log("Attack");
        thisEnemy.SetMoveSpeed(0);
    }

    public override void Update()
    {
        for (int i = 0; i < thisEnemy.GetAttackingTargets().Count; i++)
        {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0)
            {
                thisEnemy.GetAttackingTargets()[i].GetComponent<Entity>().SetHealth(thisEnemy.GetAttackingTargets()[i].GetComponent<Entity>().GetHealth() - thisEnemy.GetAttackDmg());
                attackCooldown = 1.0f;
                Debug.Log("Unit getting attacked");
            }
            if (Vector3.Distance(thisEnemy.transform.position, thisEnemy.GetAttackingTargets()[i].transform.position) > 1.0f)
            {
                thisEnemy.sm.SetNextState("Patrol");
            }
        }
    }

    public override void Exit()
    {
    }
}
