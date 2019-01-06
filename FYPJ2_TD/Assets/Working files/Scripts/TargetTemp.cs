using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTemp : Entity {

	public TargetTemp()
    {
        f_health = 5.0f;
    }
    private void Update()
    {
        if (f_health <= 0.0f)
            Destroy(gameObject);
    }
}
