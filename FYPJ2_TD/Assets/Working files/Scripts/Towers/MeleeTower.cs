using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeleeTower : Entity
{
    [SerializeField] BuildingPlot parentPlot;

    // Rallypoint related
    GameObject rallyPoint, rangeRadiusSprite, rangeRadiusCollider;
    Image rallyPointImg;
    MeleeTowerTap tapScript;
    public bool selectedRallypoint;

    //manager stats 
    GameObject canvas;

    // Soldier related
    float spawnTimer = 0.0f;
    [SerializeField] float spawnDur = 2.0f;
    [SerializeField] int soldierMaxCount = 1;
    [SerializeField] GameObject soldierPrefab;
    [SerializeField] List<GameObject> soldierList;

    //ui related
    [SerializeField] GameObject towerCanvas, upgradeCanvas, progressCanvas;
    Button towerButton;
    Slider progressSlider;


    enum MeleeLevel
    {
        LEVEL1 = 0,
        LEVEL2,
        MELEELEVEL_NUM
    }
    MeleeLevel level = MeleeLevel.LEVEL1;

    public MeleeTower()
    {
        f_health = 10.0f;
        f_attackDmg = 2.0f;
        f_attackSpeed = 3.0f;
        s_name = "Archer Tower";
    }

    private void Awake()
    {
        spawnTimer = 0.0f;

        if (transform.Find("TowerCanvas"))
        {
            towerCanvas = transform.Find("TowerCanvas").gameObject;
            towerButton = towerCanvas.transform.Find("TowerButton").GetComponent<Button>();
        }
        if (transform.Find("UpgradeCanvas"))
        {
            upgradeCanvas = transform.Find("UpgradeCanvas").gameObject;
            upgradeCanvas.SetActive(false);
            rangeRadiusSprite = upgradeCanvas.transform.Find("RangeRadius").gameObject;
            rangeRadiusSprite.SetActive(false);
            rangeRadiusCollider = transform.Find("RadiusCollider").gameObject;
        }
        if (transform.Find("ProgressCanvas"))
        {
            progressCanvas = transform.Find("ProgressCanvas").gameObject;
            progressSlider = progressCanvas.transform.Find("ProgressSlider").GetComponent<Slider>();
            progressCanvas.SetActive(true);
        }
        
        if (transform.Find("MeleeTappingManager"))
        {
            tapScript = transform.Find("MeleeTappingManager").GetComponent<MeleeTowerTap>();
            tapScript.enabled = false;

            tapScript.SetParentTower(this);
            tapScript.SetParentRadius(rangeRadiusCollider);

            GameObject cancel = upgradeCanvas.transform.Find("CancelButton").gameObject;
            tapScript.SetParentCancel(cancel);
        }

        if (transform.Find("Rallypoint"))
        {
            rallyPoint = transform.Find("Rallypoint").gameObject;
            rallyPointImg = rallyPoint.transform.Find("RallypointCanvas").transform.Find("Image").GetComponent<Image>();
            rallyPointImg.color = new Color32(255, 255, 225, 0);
        }
        
        canvas = GameObject.Find("Canvas"); //manager stats
    }

    private void Update()
    {
        SpawnSoldier();
        DisplayProgressSlider();
    }

    void SpawnSoldier()
    {
        if (soldierList.Count < soldierMaxCount && soldierPrefab != null)
        {
            spawnTimer += 1 * Time.deltaTime;
            progressSlider.value = CalculateProgress();

            if (spawnTimer > spawnDur)
            {
                GameObject go = Instantiate(soldierPrefab, rallyPoint.transform.position, Quaternion.identity);

                //go's waypoint = rallypoint
                soldierList.Add(go);
                spawnTimer = 0.0f;
            }
        }
    }

    public void SetParentPlot(BuildingPlot plot)
    {
        parentPlot = plot;
    }

    public void SellMeleeTower()
    {
        CancelUpgrade();

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

    public void SelectedRallyPointButton()
    {
        selectedRallypoint = !selectedRallypoint;
        rangeRadiusSprite.SetActive(selectedRallypoint);
        rangeRadiusCollider.SetActive(selectedRallypoint);

        tapScript.enabled = !tapScript.enabled;
    }

    public void SetRallyPoint(Vector3 _vec)
    {
        rallyPoint.transform.position = _vec;
        rallyPointImg.color = new Color(255, 255, 255, 255);
        CancelUpgrade();
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

        selectedRallypoint = false;
        rangeRadiusSprite.SetActive(false);
        rangeRadiusCollider.SetActive(false);
        tapScript.enabled = false;
    }

    void DisplayProgressSlider()
    {
        if (soldierList.Count <= 0 && !upgradeCanvas.activeSelf)
            progressCanvas.SetActive(true);
        else
            progressCanvas.SetActive(false);
    }

    float CalculateProgress() { return spawnTimer / spawnDur; }
}
