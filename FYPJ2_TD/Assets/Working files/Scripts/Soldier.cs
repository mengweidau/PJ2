using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Entity
{
    [HideInInspector]
    public StateMachine sm;
    private int numOfTargets = 1;
    public SpriteRenderer sr;
    public MeleeTower parentTower;
    public GameObject rallyPoint;

    public string currentState = "";
    public float distanceToRally;
    public Vector3 standpos;
    public float originalMoveSpeed;

    List<GameObject> clearList;
    
    // Use this for initialization
    void Start()
    {
        s_name = "Soldier";
        clearList = new List<GameObject>();
        originalMoveSpeed = f_moveSpeed;

        sm = new StateMachine();
        sm.AddState(new SoldierIdle("Idle", this));
        sm.AddState(new SoldierMove("Move", this));
        sm.AddState(new SoldierChase("Chase", this));
        sm.AddState(new SoldierAttack("Attack", this));

        sr = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        currentState = sm.GetCurrentState();

        for (int i = 0; i < GetAttackingTargets().Count; i++)
        {
            if (sm.GetCurrentState() == "Chase" && GetAttackingTargets()[i] != null)
            {
                if (GetAttackingTargets()[i].transform.position.x < transform.position.x)
                    sr.flipX = true;
                else
                    sr.flipX = false;
            }
        }

        if (GetHealth() <= 0)
        {
            Destroy(gameObject);
        }

        SetAttackingTarget();
        sm.Update();

        distanceToRally = Vector3.Distance(rallyPoint.transform.position, transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            targets.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
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

    public void SetParentTower(MeleeTower tower)
    {
        parentTower = tower;
    }
    public void SetRallyPoint(GameObject point)
    {
        rallyPoint = point;
    }

    public void SetUpgradedStats()
    {
        f_attackDmg = 1.5f;
        f_health = 4.0f;
        f_attackSpeed = 1.75f;
    }
    
    public void RefreshTargList()
    {
        //--CLEAR TARGET LIST
        GetTargets().Clear();
        //add targetlist into clear list & clear the targetlist
        for (int i = 0; i < GetTargets().Count; i++)
        {
            if (GetTargets()[i])
                clearList.Add(GetTargets()[i]);
        }
        GetTargets().Clear();

        //add back gameobjects from clearList to targList & clear the clearlist
        for (int i = 0; i < clearList.Count; i++)
            GetTargets().Add(clearList[i]);
        clearList.Clear();
    }

    public void RefreshAtkList()
    {
        //--CLEAR ATTACKING LIST
        GetTargets().Clear();
        //add atk list into clear list & clear atk list
        for (int i = 0; i < GetAttackingTargets().Count; i++)
        {
            if (GetAttackingTargets()[i])
                clearList.Add(GetAttackingTargets()[i]);
        }
        GetAttackingTargets().Clear();

        //add back gameobjects from clearList to atk list & clear the clearlist
        for (int i = 0; i < clearList.Count; i++)
            GetAttackingTargets().Add(clearList[i]);
        clearList.Clear();
    }
}
