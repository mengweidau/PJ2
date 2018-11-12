using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boy : Entity{
    
    public Boy()
    {
        s_name = "john";
        i_health = 1;
    }

    private void Update()
    {
        if (talkBuddy != null && Input.GetKeyDown(KeyCode.T))
            Talk();
    }

    public new void Talk()
    {
        if (talkBuddy.GetComponent<Entity>())
        {
            Debug.Log(s_name + " greets " + talkBuddy.GetComponent<Entity>().GetName());
        }
    }
}
