using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArcherTower : Entity {

    [SerializeField] private float attackTimer;
    [SerializeField] private int numOfTargets;
    //[SerializeField] private List<GameObject> shootingTargets;
    [SerializeField] private BuildingPlot parentPlot;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] GameObject towerCanvas, upgradeCanvas;
    private Button towerButton;

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
    }

    private void Awake()
    {
        arrowPrefab = null;
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
            
            if (arrowPrefab != null && attackTimer > i_attackSpeed)
            {
                //Debug.Log("shot");
                GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
                arrow.GetComponent<Arrow>().SetArrowTarget(attackingTargets[0]);
                arrow.GetComponent<Arrow>().SetParentTower(this);

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
        //increase (original cost of the tower * 0.85f)
        if (parentPlot != null)
            parentPlot.PlotReturn();
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
}
