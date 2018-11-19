using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Entity
{
    [HideInInspector]
    public StateMachine sm;
    public Transform targetWaypoint = null;
    public SpriteRenderer sr;
    public Animator anim;

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

        sr = gameObject.GetComponentInChildren<SpriteRenderer>();
        anim = gameObject.GetComponentInChildren<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        sm.Update();
        SetAttackingTarget();
        print(sm.GetCurrentState());

        if (targetWaypoint.transform.position.x < transform.position.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }

        for (int i = 0; i < numOfTargets; ++i)
        {
            if (sm.GetCurrentState() == "Attack")
            {
                anim.SetBool("Attack", true);
                if (GetAttackingTargets()[i].transform.position.x < transform.position.x)
                {
                    sr.flipX = true;
                }
                else
                {
                    sr.flipX = false;
                }
            }
            else if (sm.GetCurrentState() == "Chase")
            {
                if (GetAttackingTargets()[i].transform.position.x < transform.position.x)
                {
                    sr.flipX = true;
                }
                else
                {
                    sr.flipX = false;
                }
            }
            else
            {
                anim.SetBool("Chase", false);
                anim.SetBool("Attack", false);
            }
        }

        if (GetHealth() <= 0)
            Destroy(gameObject);
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
