using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySkeleton : Entity
{
    [HideInInspector]
    public StateMachine sm;
    public Transform targetWaypoint = null;
    public SpriteRenderer sr;
    public Animator anim;
    public Image healthBar;

    private int waypointIndex = 0;
    private int numOfTargets = 1;
    private float health;
    
    [SerializeField] GameObject canvas;
    [SerializeField] string currentState = "";
    List<GameObject> clearList;

    // Use this for initialization
    void Start()
    {
        targetWaypoint = Waypoints.waypoints[0];

        s_name = "Skeleton";
        health = f_health;
        clearList = new List<GameObject>();

        sm = new StateMachine();
        sm.AddState(new SkeletonPatrol("Patrol", this));
        sm.AddState(new SkeletonChase("Chase", this));
        sm.AddState(new SkeletonAttack("Attack", this));

        sr = gameObject.GetComponentInChildren<SpriteRenderer>();
        anim = gameObject.GetComponentInChildren<Animator>();
        healthBar = gameObject.GetComponentInChildren<Image>();
        canvas = GameObject.Find("Canvas");
    }


    // Update is called once per frame
    void Update()
    {
        currentState = sm.GetCurrentState();

        if (targetWaypoint.transform.position.x < transform.position.x)
            sr.flipX = true;
        else
            sr.flipX = false;

        for (int i = 0; i < GetAttackingTargets().Count; ++i)
        {
            if (sm.GetCurrentState() == "Chase" && GetAttackingTargets()[i] != null)
            {
                if (GetAttackingTargets()[i].transform.position.x < transform.position.x)
                    sr.flipX = true;
                else
                    sr.flipX = false;
            }
            if (sm.GetCurrentState() == "Attack" && GetAttackingTargets()[i]!=null)
            {
                anim.SetBool("Attack", true);
                if (GetAttackingTargets()[i].transform.position.x < transform.position.x)
                    sr.flipX = true;
                else
                    sr.flipX = false;
            }
            else
                anim.SetBool("Attack", false);
        }

        healthBar.fillAmount = GetHealth() / health;

        if (GetHealth() <= 0)
        {
            canvas.GetComponent<ManagerStats>().AddGold(100);
            Destroy(gameObject);
        }

        SetAttackingTarget();
        sm.Update();
    }

    public void GetNextwaypoint()
    {
        if (waypointIndex >= Waypoints.waypoints.Length - 1)
        {
            waypointIndex = 0;
            canvas.GetComponent<ManagerStats>().SetLife(canvas.GetComponent<ManagerStats>().GetLife() - 1);
            Debug.Log("Enemy has escaped, health decreases!");
            Destroy(gameObject);
        }
        waypointIndex++;
        targetWaypoint = Waypoints.waypoints[waypointIndex];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Enter");
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

    public void RefreshTargList()
    {
        // adds non-null gameobjects into clear list
        for (int i = 0; i < GetAttackingTargets().Count; i++)
        {
            if (GetAttackingTargets()[i])
                clearList.Add(GetAttackingTargets()[i]);
        }

        //clear the targList
        GetAttackingTargets().Clear();

        //add back gameobjects from clearList to targList
        for (int i = 0; i < clearList.Count; i++)
            GetAttackingTargets().Add(clearList[i]);

        //clear the clearList
        clearList.Clear();
    }
}
