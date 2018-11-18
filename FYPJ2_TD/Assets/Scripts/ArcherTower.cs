using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTower : Entity {

    [SerializeField] private float attackTimer;
    [SerializeField] private int numOfTargets;
    [SerializeField] private List<GameObject> shootingTargets;
    [SerializeField] private BuildingPlot parentPlot;
    [SerializeField] GameObject arrowPrefab;

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
        i_attackSpeed = 3;
        s_name = "Archer Tower";
        targets = new List<GameObject>();


        arrowPrefab = null;
        attackTimer = 0.0f;
        numOfTargets = 1;
        shootingTargets = new List<GameObject>();
    }
    
    private void Update()
    {
        DetermineShootingTargets();
        Shoot();
    }
    
    //adds enemy as a potential target
    private void OnTriggerEnter(Collider other) 
    {
        //add other.gameobj if it is an enemy/has enemy tag
        if (other.CompareTag("Enemy"))
            targets.Add(other.gameObject);
    }

    //remove potential target
    private void OnTriggerExit(Collider other)
    {
        //add other.gameobj if it is an enemy/has enemy tag
        if (other.CompareTag("Enemy"))
        {
            targets.Remove(other.gameObject);
            shootingTargets.Remove(other.gameObject);
        }
    }

    private void DetermineShootingTargets()
    {
        if (shootingTargets.Count < numOfTargets && targets.Count != 0)
        {
            for (int i = 0; i < numOfTargets; ++i)
            {
                shootingTargets.Add(targets[i]);
            }
        }
    }

    private void Shoot()
    {
        if (shootingTargets.Count > 0)
        {
            attackTimer += Time.deltaTime * 1.0f;
            //Debug.Log("archer shoot timer: " +attackTimer);
            
            if (arrowPrefab != null && attackTimer > i_attackSpeed)
            {
                //Debug.Log("shot");
                GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
                arrow.GetComponent<Arrow>().SetArrowTarget(shootingTargets[0]);
                arrow.GetComponent<Arrow>().SetParentTower(this);

                attackTimer = 0.0f;
            }
        }
    }

    public void RemoveTarget(GameObject gameobject)
    {
        for (int i = 0; i < shootingTargets.Count; ++i)
        {
            if (shootingTargets[i] == gameobject)
            {
                shootingTargets.Remove(gameobject);
                targets.Remove(gameobject);
            }
        }
    }

    public void SetParentPlot(BuildingPlot plot)
    {
        parentPlot = plot;
    }

    public void SellArcherTower()
    {
        //increase (original cost of the tower * 0.8f)
        parentPlot.PlotReturn();
    }

    //To do - add buttons func for: open/close upgrade canvas
}
