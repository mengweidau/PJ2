using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    Dictionary<string, State> stateMap = new Dictionary<string, State>();
    State currState = null;
    State nextState = null;

    // Update is called once per frame
    public void Update()
    {
        if (nextState != currState)
        {
            // Exit the currstate
            currState.Exit();
            // Set the currstate to nextstate
            currState = nextState;
            // Enter the state
            currState.Enter();
        }
        // Update the state
            currState.Update();
    }

    // Add new state
    public void AddState(State newState)
    {
        // If the newState stateID is null
        if (newState.GetStateID() == "")
            return;
        // If the stateMap contains the same stateID
        if (stateMap.ContainsKey(newState.GetStateID()))
            return;

        // If the currState is null
        if (currState == null)
        {
            nextState = newState;
            currState = nextState;
        }

        // Add state
        stateMap.Add(newState.GetStateID(), newState);
    }

    // Set next state
    public void SetNextState(string nextStateID)
    {
        // If got nextStateID, set it as nextState
        if (stateMap.ContainsKey(nextStateID))
        {
            nextState = (State)stateMap[nextStateID];
        }
    }

    // Get current state
    public string GetCurrentState()
    {
        // If currState is not null, return the currStateID
        if (currState != null)
            return currState.GetStateID();
        return "";
    }
}
