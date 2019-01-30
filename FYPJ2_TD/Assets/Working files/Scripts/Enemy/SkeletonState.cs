using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonPatrol : State
{
    EnemySkeleton thisEnemy;
    List<GameObject> clearList = new List<GameObject>();

    public SkeletonPatrol(string stateID, EnemySkeleton enemy)
    {
        m_stateID = stateID;
        thisEnemy = enemy;
    }

    public override void Enter()
    {
        //Debug.Log("SkelePatrol");
        thisEnemy.SetMoveSpeed(1);
    }

    void RefreshTargList()
    {
        // adds non-null gameobjects into clear list
        for (int i = 0; i < thisEnemy.GetAttackingTargets().Count; i++)
        {
            if (thisEnemy.GetAttackingTargets()[i])
                clearList.Add(thisEnemy.GetAttackingTargets()[i]);
        }

        //clear the targList
        thisEnemy.GetAttackingTargets().Clear();

        //add back gameobjects from clearList to targList
        for (int i = 0; i < clearList.Count; i++)
            thisEnemy.GetAttackingTargets().Add(clearList[i]);

        //clear the clearList
        clearList.Clear();
    }

    public override void Update()
    {
        RefreshTargList();

        //check direction to current waypoint
        Vector3 direction = (thisEnemy.targetWaypoint.position - thisEnemy.transform.position).normalized;

        // if-reached curr waypoint, finds next waypoint | else-move towards curr waypoint
        if (Vector3.Distance(thisEnemy.targetWaypoint.position, thisEnemy.transform.position) < 0.1f)
        {
            //Debug.Log("Finding next waypoint");
            thisEnemy.GetNextwaypoint();
        }
        else
        {
            thisEnemy.transform.Translate(direction * thisEnemy.GetMoveSpeed() * Time.deltaTime);
        }

        //cycles thru targlist to check if close enough to chase 
        for (int i = 0; i < thisEnemy.GetAttackingTargets().Count; i++)
        {
            if (Vector3.Distance(thisEnemy.GetAttackingTargets()[i].transform.position, thisEnemy.transform.position) < 1.0f)
            {
                //Debug.Log("SkeleChase");
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
    List<GameObject> clearList = new List<GameObject>();

    public SkeletonChase(string stateID, EnemySkeleton enemy)
    {
        m_stateID = stateID;
        thisEnemy = enemy;
    }

    public override void Enter()
    {
        thisEnemy.SetMoveSpeed(1);
    }

    void RefreshTargList()
    {
        // adds non-null gameobjects into clear list
        for (int i = 0; i < thisEnemy.GetAttackingTargets().Count; i++)
        {
            if (thisEnemy.GetAttackingTargets()[i])
                clearList.Add(thisEnemy.GetAttackingTargets()[i]);
        }

        //clear the targList
        thisEnemy.GetAttackingTargets().Clear();

        //add back gameobjects from clearList to targList
        for (int i = 0; i < clearList.Count; i++)
            thisEnemy.GetAttackingTargets().Add(clearList[i]);

        //clear the clearList
        clearList.Clear();
    }

    public override void Update()
    {
        RefreshTargList();

        for (int i = 0; i < thisEnemy.GetAttackingTargets().Count; i++)
        {
            if (thisEnemy.GetAttackingTargets()[i] != null)
            {
                if (Vector3.Distance(thisEnemy.GetAttackingTargets()[i].transform.position, thisEnemy.transform.position) < 0.2f)
                {
                    //Debug.Log("Close enough to attack");
                    thisEnemy.sm.SetNextState("Attack");
                }
                else if (Vector3.Distance(thisEnemy.GetAttackingTargets()[i].transform.position, thisEnemy.transform.position) < 1.0f)
                {
                    //Debug.Log("Chasing target");
                    direction = (thisEnemy.GetAttackingTargets()[i].transform.position - thisEnemy.transform.position).normalized;
                    thisEnemy.transform.Translate(direction * thisEnemy.GetMoveSpeed() * Time.deltaTime);
                }
                else
                {
                    //Debug.Log("Target out of range, return to patrol");
                    thisEnemy.sm.SetNextState("Patrol");
                }
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
    List<GameObject> clearList = new List<GameObject>();

    public SkeletonAttack(string stateID, EnemySkeleton enemy)
    {
        m_stateID = stateID;
        thisEnemy = enemy;
    }

    void RefreshTargList()
    {
        // adds non-null gameobjects into clear list
        for (int i = 0; i < thisEnemy.GetAttackingTargets().Count; i++)
        {
            if (thisEnemy.GetAttackingTargets()[i])
                clearList.Add(thisEnemy.GetAttackingTargets()[i]);
        }

        //clear the targList
        thisEnemy.GetAttackingTargets().Clear();

        //add back gameobjects from clearList to targList
        for (int i = 0; i < clearList.Count; i++)
            thisEnemy.GetAttackingTargets().Add(clearList[i]);

        //clear the clearList
        clearList.Clear();
    }

    public override void Enter()
    {
        //Debug.Log("Attack");
        thisEnemy.SetMoveSpeed(0);
    }

    public override void Update()
    {
        RefreshTargList();

        if (thisEnemy.GetAttackingTargets().Count > 0)
        {
            for (int i = 0; i < thisEnemy.GetAttackingTargets().Count; i++)
            {
                attackCooldown -= Time.deltaTime;
                if (attackCooldown <= 0)
                {
                    thisEnemy.GetAttackingTargets()[i].GetComponent<Entity>().SetHealth(thisEnemy.GetAttackingTargets()[i].GetComponent<Entity>().GetHealth() - thisEnemy.GetAttackDmg());
                    attackCooldown = 1.0f;
                }
                if (thisEnemy.GetAttackingTargets()[i].GetComponent<Entity>().GetHealth() <= 0)
                {
                    thisEnemy.GetAttackingTargets().Remove(thisEnemy.GetAttackingTargets()[i]);
                    thisEnemy.GetTargets().Remove(thisEnemy.GetTargets()[i]);
                    thisEnemy.sm.SetNextState("Patrol");
                    thisEnemy.anim.SetBool("Attack", false);
                }
                else if (Vector3.Distance(thisEnemy.GetAttackingTargets()[i].transform.position, thisEnemy.transform.position) > 1.0f)
                {
                    //Debug.Log("Target out of attack range, return to patrol");
                    thisEnemy.sm.SetNextState("Patrol");
                }
            }
        }
        else
            thisEnemy.sm.SetNextState("Patrol");
    }

    public override void Exit()
    {
    }
}

