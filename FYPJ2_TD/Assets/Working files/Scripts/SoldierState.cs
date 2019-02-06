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
        thisSoldier.SetMoveSpeed(thisSoldier.originalMoveSpeed);
    }

    public override void Update()
    {
        //check if not at standing position
        float distToStandPos = Vector3.Distance(thisSoldier.standpos, thisSoldier.transform.position);
        if (!(distToStandPos < 0.1f))
            thisSoldier.sm.SetNextState("Move");

        //check if any enemies within range
        thisSoldier.RefreshTargList();
        thisSoldier.RefreshAtkList();
        if (thisSoldier.GetAttackingTargets().Count > 0)
        {
            for (int i = 0; i < thisSoldier.GetAttackingTargets().Count; i++)
            {
                if (thisSoldier.GetAttackingTargets() != null)
                {
                    float distToTarg = Vector3.Distance(thisSoldier.GetAttackingTargets()[i].transform.position, thisSoldier.transform.position);

                    if (distToTarg < 1.0f)
                        thisSoldier.sm.SetNextState("Chase");
                }
            }
        }   
    }

    public override void Exit()
    {
    }

}

public class SoldierMove : State
{
    Soldier thisSoldier;
    Vector3 newPos;
    Vector3 direction;

    public SoldierMove(string stateID, Soldier soldier)
    {
        m_stateID = stateID;
        thisSoldier = soldier;
    }

    public override void Enter()
    {
        thisSoldier.SetMoveSpeed(thisSoldier.originalMoveSpeed);

        //set a new position that is near the rallypoint
        Vector3 tempVec = thisSoldier.rallyPoint.transform.position;
        float randX = Random.Range(-0.5f, 0.5f);
        float randZ = Random.Range(-0.5f, 0.5f);
        newPos = new Vector3(tempVec.x + randX, tempVec.y, tempVec.z + randZ);

        //set new standing position
        thisSoldier.standpos = newPos;
    }

    public override void Update()
    {
        //check if near new standing position
        if (Vector3.Distance(newPos, thisSoldier.transform.position) < 0.1f)
            thisSoldier.sm.SetNextState("Idle");
        //move towards new standing position
        else
        {
            direction = (newPos - thisSoldier.transform.position).normalized;
            thisSoldier.transform.Translate(direction * thisSoldier.GetMoveSpeed() * Time.deltaTime);
        }

        //checks for enemies, that are near thisSoldier while its moving towards its new position
        thisSoldier.RefreshTargList();
        thisSoldier.RefreshAtkList();
        if (thisSoldier.GetAttackingTargets().Count > 0)
        {
            for (int i = 0; i < thisSoldier.GetAttackingTargets().Count; i++)
            {
                if (thisSoldier.GetAttackingTargets()[i] != null)
                {
                    //get distance between targ and thisSoldier
                    float distToTarg = Vector3.Distance(thisSoldier.GetAttackingTargets()[i].transform.position, thisSoldier.transform.position);
                    float distToRally = Vector3.Distance(thisSoldier.rallyPoint.transform.position, thisSoldier.transform.position);

                    //chase if targ is within range
                    if (distToTarg < 1.0f && distToRally < 1.0f)
                        thisSoldier.sm.SetNextState("Chase");
                    else
                        thisSoldier.sm.SetNextState("Idle");
                }
            }
        }
        else
            thisSoldier.sm.SetNextState("Idle");
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
        thisSoldier.SetMoveSpeed(thisSoldier.originalMoveSpeed);
    }

    public override void Update()
    {
        thisSoldier.RefreshTargList();
        thisSoldier.RefreshAtkList();

        if (thisSoldier.GetAttackingTargets().Count > 0)
        {
            for (int i = 0; i < thisSoldier.GetAttackingTargets().Count; i++)
            {
                if (thisSoldier.GetAttackingTargets()[i] != null)
                {
                    float distToTarg = Vector3.Distance(thisSoldier.GetAttackingTargets()[i].transform.position, thisSoldier.transform.position);
                    float distToRally = Vector3.Distance(thisSoldier.rallyPoint.transform.position, thisSoldier.transform.position);

                    //checks dist to targ, and switch to attack state
                    if (distToTarg < 0.2f)
                    {
                        thisSoldier.sm.SetNextState("Attack");
                    }
                    //checks if dist to targ still in range to chase
                    else if (distToTarg < 1.0f && distToRally < 1.0f)
                    {
                        direction = (thisSoldier.GetAttackingTargets()[i].transform.position - thisSoldier.transform.position).normalized;
                        thisSoldier.transform.Translate(direction * thisSoldier.GetMoveSpeed() * Time.deltaTime);
                    }
                    else
                        thisSoldier.sm.SetNextState("Idle");
                }
            }
        }
        else
            thisSoldier.sm.SetNextState("Idle");
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
        thisSoldier.SetMoveSpeed(0);
    }

    public override void Update()
    {
        thisSoldier.RefreshTargList();
        thisSoldier.RefreshAtkList();

        if (thisSoldier.GetAttackingTargets().Count > 0)
        {
            for (int i = 0; i < thisSoldier.GetAttackingTargets().Count; i++)
            {
                if (thisSoldier.GetAttackingTargets() != null)
                {
                    attackCooldown -= Time.deltaTime;
                    float distToTarg = Vector3.Distance(thisSoldier.GetAttackingTargets()[i].transform.position, thisSoldier.transform.position);

                    if (distToTarg > 1.0f)
                        thisSoldier.sm.SetNextState("Idle");

                    //attack - dmg the target
                    if (attackCooldown <= 0)
                    {
                        thisSoldier.GetAttackingTargets()[i].GetComponent<Entity>().SetHealth(thisSoldier.GetAttackingTargets()[i].GetComponent<Entity>().GetHealth() - thisSoldier.GetAttackDmg());
                        attackCooldown = 1.0f;
                    }

                    //checks if targ is dead, if dead remove targ from list and return to idle
                    if (thisSoldier.GetAttackingTargets()[i].GetComponent<Entity>().GetHealth() <= 0 &&
                    thisSoldier.GetAttackingTargets() != null)
                    {
                        thisSoldier.GetTargets().Remove(thisSoldier.GetAttackingTargets()[i]);
                        thisSoldier.GetAttackingTargets().Remove(thisSoldier.GetAttackingTargets()[i]);
                        thisSoldier.sm.SetNextState("Idle");
                    }
                }
            }
        }
        else
            thisSoldier.sm.SetNextState("Idle");
    }

    public override void Exit()
    {
    }

}