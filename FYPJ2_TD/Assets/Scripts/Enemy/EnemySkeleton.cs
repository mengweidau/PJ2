using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Entity
{
    public Transform targetWaypoint = null;
    public Transform targetEnemy = null;
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
    }


    // Update is called once per frame
    void Update()
    {
        sm.Update();
        print(sm.GetCurrentState());

        //Vector3 direction = target.position - transform.position;
        //transform.Translate(direction.normalized * i_moveSpeed * Time.deltaTime);

        //if (Vector3.Distance(transform.position, target.position) <= 0.1f)
        //{
        //    GetNextwaypoint();
        //}
    }

    public void GetNextwaypoint()
    {
        if (waypointIndex >= Waypoints.waypoints.Length - 1)
        {
            //this.transform.position = new Vector3(-0.59f, -5.0f, -3.22f);
            waypointIndex = 0;
            Destroy(gameObject);
            Debug.Log("Enemy has escaped, health decreases!");
        }
        waypointIndex++;
        targetWaypoint = Waypoints.waypoints[waypointIndex];
    }
}
