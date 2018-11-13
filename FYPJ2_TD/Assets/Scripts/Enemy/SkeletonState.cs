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
    }

    public override void Update()
    {
        Vector3 direction = thisEnemy.targetWaypoint.position - thisEnemy.transform.position;

        if (Vector3.Distance(thisEnemy.transform.position, thisEnemy.targetWaypoint.position) <= 0.1f)
        {
            thisEnemy.GetNextwaypoint();
        }
        else if (Vector3.Distance(thisEnemy.transform.position, GameObject.FindWithTag("Player").transform.position) < 1.0f)
        {
            thisEnemy.sm.SetNextState("Chase");
        }
        else
        {
            thisEnemy.transform.Translate(direction.normalized * 1.0f * Time.deltaTime);
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
    }

    public override void Update()
    {
        thisEnemy.targetEnemy = GameObject.FindWithTag("Player").transform;
        direction = thisEnemy.targetEnemy.position - thisEnemy.transform.position;
        thisEnemy.transform.Translate(direction.normalized * 1 * Time.deltaTime);

        if (Vector3.Distance(thisEnemy.transform.position, thisEnemy.targetEnemy.position) > 1.0f)
        {
            thisEnemy.targetEnemy = null;
            thisEnemy.sm.SetNextState("Patrol");
        }
    }

    public override void Exit()
    {
    }
}
