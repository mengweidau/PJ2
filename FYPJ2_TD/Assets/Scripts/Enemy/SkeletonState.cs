using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonPatrol : State
{
    EnemySkeleton thisEnemy;

    public SkeletonPatrol(string stateID, EnemySkeleton enemy)
    {
        m_stateID = stateID;
        thisEnemy = enemy;
    }

    public override void Enter()
    {
        thisEnemy.SetMoveSpeed(1);
        Debug.Log("Patrol");
    }

    public override void Update()
    {
        Vector3 direction = thisEnemy.targetWaypoint.position - thisEnemy.transform.position;

        if (Vector3.Distance(thisEnemy.transform.position, thisEnemy.targetWaypoint.position) <= 0.1f)
        {
            thisEnemy.GetNextwaypoint();
            //Debug.Log("reached");
        }
        else
        {
            thisEnemy.transform.Translate(direction.normalized * thisEnemy.GetMoveSpeed() * Time.deltaTime);
            //Debug.Log("go");
        }

        thisEnemy.targetUnit = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject unit in thisEnemy.targetUnit)
        {
            if (Vector3.Distance(thisEnemy.transform.position, unit.transform.position) < 1.0f)
            {
                thisEnemy.sm.SetNextState("Chase");
            }
        }
    }


    public override void Exit()
    {
    }
}

public class SkeletonChase : State
{
    EnemySkeleton thisEnemy;
    Vector3 direction;

    public SkeletonChase(string stateID, EnemySkeleton enemy)
    {
        m_stateID = stateID;
        thisEnemy = enemy;
    }

    public override void Enter()
    {
        Debug.Log("Chase");
        thisEnemy.SetMoveSpeed(2);
    }

    public override void Update()
    {
        thisEnemy.targetUnit = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject unit in thisEnemy.targetUnit)
        {
            if (Vector3.Distance(thisEnemy.transform.position, unit.transform.position) > 1.0f)
            {
                thisEnemy.sm.SetNextState("Patrol");
            }
            else if (Vector3.Distance(thisEnemy.transform.position, unit.transform.position) <= 0.1f)
            {
                thisEnemy.sm.SetNextState("Attack");
            }
            else
            {
                direction = unit.transform.position - thisEnemy.transform.position;
                thisEnemy.transform.Translate(direction.normalized * thisEnemy.GetMoveSpeed() * Time.deltaTime);
            }
        }
    }

    public override void Exit()
    {
    }
}

public class SkeletonAttack : State
{
    EnemySkeleton thisEnemy;
    float attackCooldown = 1.0f;

    public SkeletonAttack(string stateID, EnemySkeleton enemy)
    {
        m_stateID = stateID;
        thisEnemy = enemy;
    }

    public override void Enter()
    {
        Debug.Log("Attack");
    }

    public override void Update()
    {
        thisEnemy.targetUnit = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject unit in thisEnemy.targetUnit)
        {           
            if (Vector3.Distance(thisEnemy.transform.position, unit.transform.position) <= 0.1f)
            {
                attackCooldown -= Time.deltaTime;
                if (attackCooldown <= 0)
                {
                    unit.GetComponent<Entity>().SetHealth(unit.GetComponent<Entity>().GetHealth() - thisEnemy.GetAttackDmg());
                    attackCooldown = 1.0f;
                    Debug.Log("Unit getting attacked");
                }
            }
            else
            {
                thisEnemy.sm.SetNextState("Patrol");
            }
        }
    }

    public override void Exit()
    {
    }
}
