using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Entity
{
    public StateMachine sm;
    public Transform targetWaypoint = null;

    private int waypointIndex = 0;
    private int numOfTargets = 1;

    // Use this for initialization
    void Start()
    {
        targetWaypoint = Waypoints.waypoints[0];
        s_name = "Skeleton";
        i_health = 16;
        i_attackDmg = 1;
        i_attackSpeed = 1;
        i_moveSpeed = 1;

        sm = new StateMachine();
        sm.AddState(new SkeletonPatrol("Patrol", this));
        sm.AddState(new SkeletonChase("Chase", this));
        sm.AddState(new SkeletonAttack("Attack", this));
    }


    // Update is called once per frame
    void Update()
    {
        sm.Update();
        SetAttackingTarget();
        print(sm.GetCurrentState());
    }

    public void GetNextwaypoint()
    {
        if (waypointIndex >= Waypoints.waypoints.Length - 1)
        {
            waypointIndex = 0;
            Destroy(gameObject);
            Debug.Log("Enemy has escaped, health decreases!");
        }
        waypointIndex++;
        targetWaypoint = Waypoints.waypoints[waypointIndex];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enter");
            targets.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Exit");
            targets.Remove(other.gameObject);
            attackingTargets.Remove(other.gameObject);
        }
    }

    public void SetAttackingTarget()
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
