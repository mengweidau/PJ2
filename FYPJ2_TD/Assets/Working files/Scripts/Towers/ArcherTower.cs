using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArcherTower : Entity {

    [SerializeField] float attackTimer;
    [SerializeField] int numOfTargets;
    [SerializeField] BuildingPlot parentPlot;
    [SerializeField] GameObject arrowPrefab;

    //ui related
    [SerializeField] Sprite upgradeImg;
    [SerializeField] GameObject towerCanvas, upgradeCanvas;
    Button towerButton, upgradeButton;

    ManagerStats managerStats;

    enum ArcherLevel
    {
        Level1 = 0,
        Level2
    }
    ArcherLevel level = ArcherLevel.Level1;

    private void Awake()
    {
        s_name = "Archer Tower";
        f_attackDmg = 1.0f;
        f_attackSpeed = 2.0f;

        attackTimer = 0.0f;
        numOfTargets = 1;

        InitialiseCanvas();
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

    private void InitialiseCanvas()
    {
        if (transform.Find("TowerCanvas"))
        {
            towerCanvas = transform.Find("TowerCanvas").gameObject;
            towerButton = towerCanvas.transform.Find("TowerButton").GetComponent<Button>();
        }
        if (transform.Find("UpgradeCanvas"))
        {
            upgradeCanvas = transform.Find("UpgradeCanvas").gameObject;
            upgradeCanvas.SetActive(false);
            upgradeButton = upgradeCanvas.transform.Find("UpgradeButton").GetComponent<Button>();
        }
        managerStats = GameObject.Find("Canvas").GetComponent<ManagerStats>();
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

            if (arrowPrefab != null && attackTimer > f_attackSpeed)
            {
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
        managerStats.AddGold(gold);
        managerStats.AddLumber(wood);

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

    public void UpgradeTowerBtn()
    {
        if (managerStats.GetGold() >= 100 && managerStats.GetLumber() >= 25)
        {
            managerStats.MinusGold(100);
            managerStats.MinusLumber(25);

            level = ArcherLevel.Level2;
            
            towerButton.GetComponent<Image>().sprite = upgradeImg;
            upgradeButton.gameObject.SetActive(false);
            upgradeCanvas.SetActive(false);
            towerButton.interactable = true;

            f_attackDmg = 2.0f;
            f_attackSpeed = 2.5f;
        }
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
