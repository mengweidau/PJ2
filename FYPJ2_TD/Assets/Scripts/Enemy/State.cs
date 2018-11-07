using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State 
{
    public string m_stateID;

    public State()
    {
        m_stateID = "";
    }

    public State(string stateID)
    {
        m_stateID = stateID;
    }

    public string GetStateID()
    {
        return m_stateID;
    }

    public virtual void Enter()
    {
    }

    public virtual void Update()
    {
    }

    public virtual void Exit()
    {
    }
}
