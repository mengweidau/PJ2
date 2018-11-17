using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Entity
{
    public Transform targetWaypoint = null;
    public GameObject[] targetUnit = null;
    private int waypointIndex = 0;

    public StateMachine sm;

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
        //print(sm.GetCurrentState());
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
}
