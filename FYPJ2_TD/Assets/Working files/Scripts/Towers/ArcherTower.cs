﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArcherTower : Entity {

    [SerializeField] float attackTimer;
    [SerializeField] int numOfTargets;
    //[SerializeField] private List<GameObject> shootingTargets;
    [SerializeField] BuildingPlot parentPlot;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] GameObject towerCanvas, upgradeCanvas;
    Button towerButton;
    GameObject canvas;

    enum ArcherLevel
    {
        Level1 = 0,
        Level2
    }
    ArcherLevel level = ArcherLevel.Level1;

	public ArcherTower()
    {
        f_health = 10.0f;
        f_attackDmg = 2.0f;
        f_attackSpeed = 3.0f;
        s_name = "Archer Tower";
    }

    private void Awake()
    {
        attackTimer = 0.0f;
        numOfTargets = 1;

        if (transform.Find("TowerCanvas"))
        {
            towerCanvas = transform.Find("TowerCanvas").gameObject;
            towerButton = towerCanvas.transform.Find("TowerButton").GetComponent<Button>();
        }
        if (transform.Find("UpgradeCanvas"))
        {
            upgradeCanvas = transform.Find("UpgradeCanvas").gameObject;
            upgradeCanvas.SetActive(false);
        }
        canvas = GameObject.Find("Canvas");
    }

    private void Update()
    {
        CheckNullTargets();
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
            attackingTargets.Remove(other.gameObject);
        }
    }

    private void DetermineShootingTargets()
    {
        if (attackingTargets.Count < numOfTargets && targets.Count != 0)
        {
            for (int i = 0; i < numOfTargets; ++i)
            {
                attackingTargets.Add(targets[i]);
            }
        }
    }

    private void Shoot()
    {
        if (attackingTargets.Count > 0)
        {
            attackTimer += Time.deltaTime * 1.0f;
            //Debug.Log("archer shoot timer: " +attackTimer);

            if (arrowPrefab != null && attackTimer > f_attackSpeed)
            {
                //Debug.Log("shot");
                AudioManager.instance.PlayShoot();
                GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
                arrow.GetComponent<Arrow>().SetParentTower(this);
                arrow.GetComponent<Arrow>().SetArrowTarget(attackingTargets[0]);

                attackTimer = 0.0f;
            }
        }
    }

    public void RemoveTarget(GameObject gameobject)
    {
        for (int i = 0; i < attackingTargets.Count; ++i)
        {
            if (attackingTargets[i] == gameobject)
            {
                attackingTargets.Remove(gameobject);
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
        //determine return value
        int gold = parentPlot.GetArrowtowerGCost();
        int wood = parentPlot.GetArrowtowerLCost();

        gold = (int)(gold * 0.25f);
        wood = (int)(wood * 0.25f);

        //return cost
        canvas.GetComponent<ManagerStats>().AddGold(gold);
        canvas.GetComponent<ManagerStats>().AddLumber(wood);

        //destroy this tower
        parentPlot.PlotReturn();
        parentPlot.b_built = false;
        Destroy(gameObject);
    }

    public void SelectedTower()
    {
        upgradeCanvas.SetActive(true);
        towerButton.interactable = false;
    }

    public void CancelUpgrade()
    {
        upgradeCanvas.SetActive(false);
        towerButton.interactable = true;
    }

    void CheckNullTargets()
    {
        if (targets.Count > 0)
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                if (targets[i] == null)
                    targets.Remove(targets[i]);
            }
        }

        if (attackingTargets.Count > 0)
        {
            for (int i = 0; i < attackingTargets.Count; ++i)
            {
                if (attackingTargets[i] == null)
                    attackingTargets.Remove(attackingTargets[i]);
            }
        }
    }
}
