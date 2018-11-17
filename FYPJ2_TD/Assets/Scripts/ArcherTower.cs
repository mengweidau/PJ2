using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTower : Entity {

    [SerializeField] private float attackTimer;

    enum ArcherLevel
    {
        Level1 = 0,
        Level2
    }
    ArcherLevel level = ArcherLevel.Level1;

	public ArcherTower()
    {
        i_health = 10;
        i_attackDmg = 2;
        i_attackSpeed = 5;
        s_name = "Archer Tower";
    }
    private void Update()
    {
        switch (level)
        {
            case ArcherLevel.Level1:
                //check if target is avail, check if target is already taken
                break;
        }
    }
}
