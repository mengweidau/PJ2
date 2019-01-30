using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierIdle : State
{
    Soldier thisSoldier;

    public SoldierIdle(string stateID, Soldier soldier)
    {
        m_stateID = stateID;
        thisSoldier = soldier;
    }

    public override void Enter()
    {
        Debug.Log("Idle");
    }

    public override void Update()
    {
        for (int i = 0; i < thisSoldier.GetAttackingTargets().Count; i++)
        {
            if (Vector3.Distance(thisSoldier.GetAttackingTargets()[i].transform.position, thisSoldier.transform.position) < 1.0f)
            {
                thisSoldier.sm.SetNextState("Chase");
            }
            //else if (Vector3.Distance(thisSoldier.GetAttackingTargets()[i].transform.position, thisSoldier.transform.position) > 1.0f)
            //{
            //    thisSoldier.transform.Translate(thisSoldier.parentTower.SavedRallypoint() * thisSoldier.GetMoveSpeed() * Time.deltaTime);
            //}
        }

    }

    public override void Exit()
    {
    }

}

public class SoldierChase : State
{
    Soldier thisSoldier;
    Vector3 direction;

    public SoldierChase(string stateID, Soldier soldier)
    {
        m_stateID = stateID;
        thisSoldier = soldier;
    }

    public override void Enter()
    {
        Debug.Log("SChase");
    }

    public override void Update()
    {
        for (int i = 0; i < thisSoldier.GetAttackingTargets().Count; i++)
        {

            if (Vector3.Distance(thisSoldier.GetAttackingTargets()[i].transform.position, thisSoldier.transform.position) < 0.2f)
            {
                thisSoldier.sm.SetNextState("Attack");
            }
            else if (Vector3.Distance(thisSoldier.GetAttackingTargets()[i].transform.position, thisSoldier.transform.position) < 1.0f)
            {
                direction = (thisSoldier.GetAttackingTargets()[i].transform.position - thisSoldier.transform.position).normalized;
                thisSoldier.transform.Translate(direction * thisSoldier.GetMoveSpeed() * Time.deltaTime);
            }
            else
            {
                thisSoldier.sm.SetNextState("Idle");
            }
        }
    }

    public override void Exit()
    {
    }

}

public class SoldierAttack : State
{
    Soldier thisSoldier;
    float attackCooldown = 1.0f;

    public SoldierAttack(string stateID, Soldier soldier)
    {
        m_stateID = stateID;
        thisSoldier = soldier;
    }

    public override void Enter()
    {
        Debug.Log("SAttack");
        thisSoldier.SetMoveSpeed(0);
    }

    public override void Update()
    {
        for (int i = 0; i < thisSoldier.GetAttackingTargets().Count; i++)
        {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0)
            {
                thisSoldier.GetAttackingTargets()[i].GetComponent<Entity>().SetHealth(thisSoldier.GetAttackingTargets()[i].GetComponent<Entity>().GetHealth() - thisSoldier.GetAttackDmg());
                attackCooldown = 1.0f;
            }
            if (thisSoldier.GetAttackingTargets()[i].GetComponent<Entity>().GetHealth() <= 0)
            {
                thisSoldier.GetAttackingTargets().Remove(thisSoldier.GetAttackingTargets()[i]);
                thisSoldier.GetTargets().Remove(thisSoldier.GetTargets()[i]);
                thisSoldier.sm.SetNextState("Idle");
            }
            else if (Vector3.Distance(thisSoldier.GetAttackingTargets()[i].transform.position, thisSoldier.transform.position) > 1.0f)
            {
                thisSoldier.sm.SetNextState("Idle");
            }
        }
    }

    public override void Exit()
    {
    }

}