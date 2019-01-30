using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTroll : Entity
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

    EnemyTroll()
    {
        s_name = "Troll";
    }

    // Use this for initialization
    void Start()
    {
        targetWaypoint = Waypoints.waypoints[0];

        health = f_health;

        sm = new StateMachine();
        sm.AddState(new TrollPatrol("Patrol", this));
        sm.AddState(new TrollChase("Chase", this));
        sm.AddState(new TrollAttack("Attack", this));

        sr = gameObject.GetComponentInChildren<SpriteRenderer>();
        anim = gameObject.GetComponentInChildren<Animator>();
        healthBar = gameObject.GetComponentInChildren<Image>();
        canvas = GameObject.Find("Canvas");
    }


    // Update is called once per frame
    void Update()
    {
        if (targetWaypoint.transform.position.x < transform.position.x)
            sr.flipX = true;
        else
            sr.flipX = false;

        for (int i = 0; i < GetAttackingTargets().Count; ++i)
        {
            if (sm.GetCurrentState() == "Attack" && GetAttackingTargets()[i] != null)
            {
                anim.SetBool("Attack", true);
                if (GetAttackingTargets()[i].transform.position.x < transform.position.x)
                    sr.flipX = true;
                else
                    sr.flipX = false;
            }
            if (sm.GetCurrentState() == "Chase" && GetAttackingTargets()[i] != null)
            {
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
            canvas.GetComponent<ManagerStats>().AddGold(200);
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
            canvas.GetComponent<ManagerStats>().SetLife(canvas.GetComponent<ManagerStats>().GetLife() - 2);
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
