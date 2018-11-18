using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTemp : Entity {

	public TargetTemp()
    {
        i_health = 5;
    }
    private void Update()
    {
        if (i_health <= 0)
            Destroy(gameObject);
    }
}
